using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewMovementScript : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public PointSystem points;

    [Header("Mobile UI")]
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [Tooltip("Компонент HoldButton на UI-кнопке поворота влево")]
    [SerializeField] private HoldButton turnLeftButton;  
    [Tooltip("Компонент HoldButton на UI-кнопке поворота вправо")]
    [SerializeField] private HoldButton turnRightButton;

    [Header("Move Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;        
    [SerializeField] private float maxSpeed = 8f;     
    [SerializeField] private bool buttonChange;

    [Header("Tricks & Air Time")]
    [SerializeField] private TMP_Text AirText;
    [SerializeField] private float AirTime;
    [SerializeField] private float removeTimer;
    [SerializeField] private bool isRemoving;

    [Header("Skate Physics & Grip")]
    [Tooltip("Насколько сильно колеса цепляются за асфальт при повороте (0 = мыло/лёд, 10+ = четкие рельсы)")]
    [SerializeField] private float driftGrip = 5f; 

    [Header("Air Move Limits")]
    [SerializeField] private int maxAirPushes = 2;
    private int airPushesLeft;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float minVelocityToRotate = 0.5f;

    [Header("Jump & Ground Check")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    [Header("Stuck Prevention")]
    [SerializeField] private float maxTiltAngle = 35f;
    [SerializeField] private float alignToGroundSpeed = 5f;

    [Header("AirPump")]
    [SerializeField] private Image pump;
    [SerializeField] private Sprite[] pumps;

    // Флаги для ввода
    private bool jumpRequested;
    private bool pushRequested;
    private float moveXInput;

    private void Start()
    {
        if (leftButton != null) leftButton.SetActive(true);
        if (rightButton != null) rightButton.SetActive(false);
        if (AirText != null) AirText.gameObject.SetActive(false);

        buttonChange = false;
        airPushesLeft = maxAirPushes;

        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        CheckGrounded();

        // Считаем время полета
        if (!isGrounded)
        {
            AirTime += Time.deltaTime;
        }
        else if (AirTime > 0)
        {
            if (AirTime > 3f)
            {
                if (AirText != null)
                {
                    AirText.gameObject.SetActive(true);
                    AirText.text = "БОНУС ЗА ВРЕМЯ В ВОЗДУХЕ (" + AirTime.ToString("F1") + "s)";
                }

                int coin = (int)AirTime;
                if (points != null) points.AddPoints(coin);

                removeTimer = 0;
                isRemoving = true;
            }

            AirTime = 0;
        }

        // Таймер скрытия текста
        if (isGrounded && isRemoving)
        {
            removeTimer += Time.deltaTime;
            if (removeTimer > 3f)
            {
                if (AirText != null) AirText.gameObject.SetActive(false);
                removeTimer = 0;
                isRemoving = false;
            }   
        }

        // Ввод поворота (Клавиатура + Мобильные кнопки)
        HandleRotationInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        if (!buttonChange && Input.GetKeyDown(KeyCode.Q))
        {
            PlayPushUIEffects(1, 1);
            TryRequestPush();
        }
        else if (buttonChange && Input.GetKeyDown(KeyCode.E))
        {
            PlayPushUIEffects(0, 2);
            TryRequestPush();
        }
    }

    /// <summary>
    /// Считывает ввод поворота с клавиатуры и с мобильных UI кнопок зажатия
    /// </summary>
    private void HandleRotationInput()
    {
        // 1. Клавиатура (A/D или стрелки)
        float keyboardInput = Input.GetAxis("Horizontal");

        // 2. Мобильные кнопки через HoldButton
        float mobileInput = 0f;

        if (turnLeftButton != null && turnLeftButton.IsPressed)
        {
            mobileInput -= 1f;
        }

        if (turnRightButton != null && turnRightButton.IsPressed)
        {
            mobileInput += 1f;
        }

        moveXInput = Mathf.Clamp(keyboardInput + mobileInput, -1f, 1f);
    }

    private void FixedUpdate()
    {
        Vector3 currentVel = rb.linearVelocity;
        Vector3 horizontalVel = new Vector3(currentVel.x, 0, currentVel.z);

        // 1. Поворот скейта
        if (horizontalVel.magnitude > minVelocityToRotate)
        {
            transform.Rotate(
                Vector3.up,
                moveXInput * rotationSpeed * Time.fixedDeltaTime,
                Space.World
            );
        }

        // 2. Сцепление колес с асфальтом (Grip)
        if (isGrounded && horizontalVel.magnitude > 0.1f)
        {
            Vector3 targetVelDirection = transform.forward * horizontalVel.magnitude;
            Vector3 newHorizontalVel = Vector3.MoveTowards(horizontalVel, targetVelDirection, driftGrip * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector3(newHorizontalVel.x, currentVel.y, newHorizontalVel.z);
        }

        // 3. Обработка толчка
        if (pushRequested)
        {
            ApplyPushForce();
            pushRequested = false;
        }

        // 4. Прыжок
        if (jumpRequested)
        {
            ExecuteJump();
            jumpRequested = false;
        }

        if (isGrounded)
        {
            AutoUprightSkate();
        }
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;

        bool centerCheck = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );

        bool rayCheck = Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.3f, groundLayer);
        bool isStuck = rb.linearVelocity.magnitude < 0.2f && Physics.Raycast(transform.position, transform.forward, 0.6f, groundLayer);

        isGrounded = centerCheck || rayCheck || isStuck;

        if (isGrounded && !wasGrounded)
        {
            airPushesLeft = maxAirPushes;
        }
    }

    public void TryRequestPush()
    {
        bool completelyStuck = rb.linearVelocity.magnitude < 0.1f;

        if (isGrounded || completelyStuck)
        {
            pushRequested = true;
        }
        else if (airPushesLeft > 0)
        {
            airPushesLeft--;
            pushRequested = true;
        }
    }

    private void ApplyPushForce()
    {
        buttonChange = !buttonChange;
        if (leftButton != null && rightButton != null)
        {
            leftButton.SetActive(!buttonChange);
            rightButton.SetActive(buttonChange);
        }
        
        Push();
    }

    public void Push()
    {
        Vector3 pushDirection = transform.forward; 
        pushDirection.y = 0f;
        pushDirection.Normalize();

        rb.AddForce(pushDirection * -speed, ForceMode.Impulse);

        Vector3 currentVel = rb.linearVelocity;
        Vector3 finalHorizontalVel = new Vector3(currentVel.x, 0, currentVel.z);

        if (finalHorizontalVel.magnitude > maxSpeed)
        {
            finalHorizontalVel = finalHorizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(finalHorizontalVel.x, currentVel.y, finalHorizontalVel.z);
        }
    }

    public void ExecuteJump()
    {
        if (isGrounded)
        {
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = 0f;
            rb.linearVelocity = currentVelocity;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (anim != null) anim.Play("SkateJumpAnimation");
            if (sound != null && sounds != null && sounds.Length > 0) sound.PlayOneShot(sounds[0]);

            isGrounded = false;
        }
    }

    private void AutoUprightSkate()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);

        if (angle > maxTiltAngle)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alignToGroundSpeed * Time.fixedDeltaTime);
        }
    }

    private void PlayPushUIEffects(int pumpSpriteIndex, int soundIndex)
    {
        if (pump != null && pumps != null && pumps.Length > pumpSpriteIndex)
            pump.sprite = pumps[pumpSpriteIndex];

        if (sound != null && sounds != null && sounds.Length > soundIndex)
            sound.PlayOneShot(sounds[soundIndex]);
    }

    public void SetPushForce(float newSpeed) => speed = newSpeed;
    public void SetMaxSpeed(float newMaxSpeed) => maxSpeed = newMaxSpeed;
    public void SetJumpForce(float newJumpForce) => jumpForce = newJumpForce;

    public void LeftButtonHandle()
    {
        if (!buttonChange)
        {
            PlayPushUIEffects(1, 1);
            TryRequestPush();
        }
    }

    public void RightButtonHandle()
    {
        if (buttonChange)
        {
            PlayPushUIEffects(0, 2);
            TryRequestPush();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}