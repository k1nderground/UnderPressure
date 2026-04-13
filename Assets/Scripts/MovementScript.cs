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

    bool isMoving = horizontalVelocity.magnitude > 0.1f;

    if (isMoving)
    {
        transform.Rotate(Vector3.up, moveX * cameraRotationSpeed * Time.deltaTime, Space.World);
    }

    if(ps.isDraining){
        Move();
    }
    }

    public void Move()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 velocity = rb.linearVelocity;
        Vector3 horizontal = new Vector3(velocity.x, 0, velocity.z);

        if (horizontal.magnitude < maxSpeed)
        {
            rb.AddForce(forward * moveMultiplier, ForceMode.Acceleration);
        }

        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        localVelocity.x = 0;
        rb.linearVelocity = transform.TransformDirection(localVelocity);
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