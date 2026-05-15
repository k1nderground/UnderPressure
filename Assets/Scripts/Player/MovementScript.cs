using UnityEngine;
using TMPro;

public class MovementScript : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Animator anim;
    [SerializeField] PressureScript ps;
    [SerializeField] PointSystem points;
    [SerializeField] AudioSource src;
    [SerializeField] SoundScript sound;

    [Header("Vars")]
    [SerializeField] float speed = 100f;
    [SerializeField] int jumpForce = 250;
    [SerializeField] int cameraRotationSpeed = 100;
    [SerializeField] int rotationSpeed = 100;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.3f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] int PhoneButton;

    [Header("SpeedCurve")]
    [SerializeField] float acceleration = 20f;
    [SerializeField] float groundBrake = 10f;

    [Header("Air Settings")]
    [SerializeField] float airDrag = 0.5f;
    [SerializeField] float airControl = 5f;

    [Header("Tricks")]
    [SerializeField] KeyCode[] KeyCodes;
    [SerializeField] int currentTrickIndex;
    [SerializeField] TMP_Text AirText;
    [SerializeField] float AirTime;
    [SerializeField] float removeTimer;
    [SerializeField] bool isRemoving;

    [Header("Other")]
    public LayerMask groundLayer;

    private bool isGrounded;
    private Rigidbody rb;

    [SerializeField] private float moveX;
    private float moveZ;
    private float moveMultiplier = 0f;

    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AirText.gameObject.SetActive(false);

        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(isGrounded && AirTime != 0)
        {
            if (AirTime > 2f)
            {
                AirText.gameObject.SetActive(true);
                AirText.text = "БОНУС ЗА ВРЕМЯ В ВОЗДУХЕ ("+AirTime+")";
                int coin = (int)AirTime;
                points.AddPoints(coin);
                removeTimer = 0;
                isRemoving = true;

            }

            AirTime = 0;
        }

        if(isGrounded){
            if (isRemoving)
            {
                removeTimer += Time.deltaTime;
                if(removeTimer > 3f)
                {
                    Debug.Log("Текст ушел");
                    AirText.gameObject.SetActive(false);
                    removeTimer = 0;
                    isRemoving = false;
                    
                }   
                
            } 
        }

        if (!isGrounded)
        {
            AirTime+=Time.deltaTime;
            Trick();
        }
    }

    void FixedUpdate()
    {
        if(PhoneButton == 1){
            moveX = 1f;
        }
        else if(PhoneButton == -1){
            moveX = -1f;
        }
        else if(PhoneButton == 0){
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
        }

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );

        if (ps.isDraining)
        {
            transform.Rotate(
                Vector3.up,
                moveX * cameraRotationSpeed * Time.deltaTime,
                Space.World
            );
        }

        Move();
    }

    public void Move()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 targetVelocity = forward * moveMultiplier;

        if (ps.isDraining)
        {
            if (isGrounded)
            {
                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetVelocity,
                    acceleration * Time.deltaTime
                );
            }
            else
            {
                currentVelocity = Vector3.Lerp(
                    currentVelocity,
                    targetVelocity,
                    airControl * Time.deltaTime
                );
            }
        }
        else
        {
            if (isGrounded)
            {
                currentVelocity = Vector3.MoveTowards(
                    currentVelocity,
                    Vector3.zero,
                    groundBrake * Time.deltaTime
                );
            }
            else
            {
                currentVelocity *= (1f - airDrag * Time.deltaTime);
            }
        }

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity =
                currentVelocity.normalized * maxSpeed;
        }

        Vector3 move = currentVelocity * Time.deltaTime;

        rb.MovePosition(
            rb.position + new Vector3(move.x, 0f, move.z)
        );
    }

    public void Jump()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );

        Debug.DrawRay(
            groundCheck.position,
            Vector3.down * groundDistance,
            Color.red
        );

        if (isGrounded)
        {
            Vector3 jumpVector = Vector3.up * jumpForce;

            rb.AddForce(jumpVector, ForceMode.Impulse);

            anim.Play("SkateJumpAnimation");
            sound.Play(1);

            Debug.Log("На земле");
        }
        else
        {
            Debug.Log("В воздухе");
        }
    }

    public void SetMoveMultiplier(float value)
    {
        moveMultiplier = value;
    }

    public void MoveLeft(){
        PhoneButton = -1;
    }

    public void MoveRight(){
        PhoneButton = 1;
    }

    public void FingerUp(){
        PhoneButton = 0;
    }

    void Trick()
    {
        if (KeyCodes.Length == 0) return;

        if (Input.GetKeyDown(KeyCodes[currentTrickIndex]))
        {
            currentTrickIndex++;

            if (currentTrickIndex >= KeyCodes.Length)
            {
                anim.Play("Skate360");
                sound.Play(2);
                points.AddPoints(25);
                currentTrickIndex = 0;
            }
        }
        else
        {
            foreach (KeyCode key in KeyCodes)
            {
                if (Input.GetKeyDown(key))
                {
                    currentTrickIndex = 0;
                    break;
                }
            }
        }
    }

    void HandleRideSound()
    {
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f;

        bool isMoving = horizontalVelocity.magnitude > 0.2f;

        if (isMoving && isGrounded)
        {
            if (!src.isPlaying)
            {
                src.Play();
            }
        }
        else
        {
            if (src.isPlaying)
            {
                src.Stop();
            }
        }
    }
}