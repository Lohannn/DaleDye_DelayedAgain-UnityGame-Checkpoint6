using System.Collections;
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

    [Header("Power Up Settings")]
    [SerializeField] private float timedPowerUpMaxTime;
    private float currentSugarTime;
    private float currentBeerTime;

    [Header("Soda Boost Settings")]
    private bool isCarryingSoda;
    [SerializeField] private float boostForce;
    [SerializeField] private float boostDuration;
    private bool isDashing;

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
        OnDash();
    }

    private void PlayerInputs()
    {
        if (isDashing) return;

        movement = Input.GetAxisRaw("Horizontal") * (!isSlowed ? currentSpeed : currentSpeed / slowDivider);

        Jump();
        Dash();
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

    private void Dash()
    {
        if (isCarryingSoda && Input.GetButtonDown("Fire3"))
        {
            isCarryingSoda = false;
            isDashing = true;
            rb.linearVelocity = new(0, rb.linearVelocityY);
        }
    }

    private void OnDash()
    {
        if (isDashing)
        {
            rb.AddForce((transform.rotation.y == 0 ? Vector2.right : Vector2.left) * boostForce,
                ForceMode2D.Impulse);
            StartCoroutine(DashTime(boostDuration));
        }
    }

    private IEnumerator DashTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDashing = false;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && OnGround())
        {
            pam.Jump();
            currentJumpTime = maxJumpTime;
        }
        else if (Input.GetButtonDown("Jump") && isCarryingSoda)
        {
            currentJumpTime = maxJumpTime;
            isCarryingSoda = false;
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

    private IEnumerator ActivateBeer()
    {
        if (currentBeerTime == 0)
        {
            currentBeerTime = timedPowerUpMaxTime;

            Time.timeScale = 0.3f;

            while (currentBeerTime > 0)
            {
                currentBeerTime--;
                yield return new WaitForSecondsRealtime(1.0f);
            }

            Time.timeScale = 1.0f;
        }
    }

    private IEnumerator ActivateSugar()
    {
        if (currentSugarTime == 0)
        {
            currentSugarTime = timedPowerUpMaxTime;

            currentSpeed = baseSpeed * 2;

            while (currentSugarTime > 0)
            {
                currentSugarTime--;
                yield return new WaitForSecondsRealtime(1.0f);
            }

            currentSpeed = baseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SewerWater"))
        {
            isSlowed = true;
        } 
        else if (collision.CompareTag("BeerPowerUp"))
        {
            StartCoroutine(ActivateBeer());

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SugarPowerUp"))
        {
            StartCoroutine(ActivateSugar());

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SodaPowerUp"))
        {
            isCarryingSoda = true;

            collision.gameObject.SetActive(false);
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
