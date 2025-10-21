using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float baseSpeed;
    private float currentSpeed;
    [SerializeField] private float slowDivider;
    private float movement;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpTime;
    private float currentJumpTime;

    private bool isSlowed;

    [Header("Sensor Ground Settings")]
    [SerializeField] private Transform groundSensor;
    [SerializeField] private Vector2 groundSensorSize;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Color32 sensorColor;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerAnimatorManager pam;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        pam = GetComponent<PlayerAnimatorManager>();

        currentSpeed = baseSpeed;
    }

    void Update()
    {
        PlayerInputs();

        Look();
    }

    void FixedUpdate()
    {
        OnMove();
        OnJump();
    }

    private void PlayerInputs()
    {
        movement = Input.GetAxisRaw("Horizontal") * (!isSlowed ? currentSpeed : currentSpeed / slowDivider);

        if (Input.GetButtonDown("Jump") && OnGround())
        {
            pam.Jump();
            currentJumpTime = maxJumpTime;
        }
        else if (Input.GetButton("Jump") && currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime;
        }
        else
        {
            currentJumpTime = 0;
        }
    }

    private void OnMove()
    {
        rb.linearVelocityX = movement;
    }

    private void Look()
    {
        if (movement > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnJump()
    {
        if (currentJumpTime > 0)
        {
            rb.linearVelocityY = jumpForce;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetMovement()
    {
        return movement;
    }

    public bool GetSlowed()
    {
        return isSlowed;
    }

    public float GetAirSpeed()
    {
        return rb.linearVelocityY;
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(groundSensor.position, groundSensorSize, 0, GroundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SewerWater"))
        {
            isSlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SewerWater"))
        {
            isSlowed = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sensorColor;
        Gizmos.DrawCube(groundSensor.position, (Vector3)groundSensorSize);
    }
}
