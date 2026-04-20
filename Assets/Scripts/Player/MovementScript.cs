using UnityEngine;

public class MovementScript : MonoBehaviour
{
[Header("Connections")]
[SerializeField] Transform cameraTransform;
[SerializeField] Animator anim;
[SerializeField] PressureScript ps;

[Header("Vars")]
[SerializeField] float speed = 100f;
[SerializeField] int jumpForce = 250;
[SerializeField] int cameraRotationSpeed = 100;
[SerializeField] int rotationSpeed = 100;
[SerializeField] Transform groundCheck;
[SerializeField] float groundDistance = 0.3f;
[SerializeField] float maxSpeed = 10f;

[Header("SpeedCurve")]
[SerializeField] float acceleration = 20f;
[SerializeField] float brake = 10f;
Vector3 currentVelocity;

public LayerMask groundLayer;

private bool isGrounded;
private Rigidbody rb;
private float moveX;
private float moveZ;
private float moveMultiplier = 0f;

void Start()
{
    rb = GetComponent<Rigidbody>();
    rb.centerOfMass = new Vector3(0, -0.5f, 0);
    rb.interpolation = RigidbodyInterpolation.Interpolate;
}

void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
    moveX = Input.GetAxis("Horizontal");
    moveZ = Input.GetAxis("Vertical");

    Vector3 horizontalVelocity = rb.linearVelocity;
    horizontalVelocity.y = 0;

    if (ps.isDraining)
    {
        transform.Rotate(Vector3.up, moveX * cameraRotationSpeed * Time.deltaTime, Space.World);
    }

        Move();
    }

   public void Move()
{
    Vector3 forward = cameraTransform.forward;
    forward.y = 0;
    forward.Normalize();

    Vector3 targetVelocity = forward * moveMultiplier;

    if (ps.isDraining)
    {
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );
    }
    else
    {
        currentVelocity = Vector3.MoveTowards(
        currentVelocity,
        Vector3.zero,
        brake * Time.deltaTime
        );

    }
    if (currentVelocity.magnitude > maxSpeed)
    {
        currentVelocity = currentVelocity.normalized * maxSpeed;
    }
    Vector3 move = currentVelocity * Time.deltaTime;

    Vector3 newPosition = new Vector3(
        transform.position.x + move.x,
        rb.position.y,
        transform.position.z + move.z
    );

    rb.MovePosition(newPosition);
}
   void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);

        if (isGrounded)
        {
            Vector3 jumpVector = Vector3.up * jumpForce;
            rb.AddForce(jumpVector, ForceMode.Impulse);
            anim.Play("SkateJumpAnimation");
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

}