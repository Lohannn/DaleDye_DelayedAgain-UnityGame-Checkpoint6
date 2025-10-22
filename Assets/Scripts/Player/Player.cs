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
    [SerializeField] private float sugarSpeedMultiplier;
    [Range(0.0f, 1.0f)][SerializeField] private float beerTimeScale;
    private float currentSugarTime;
    private float currentBeerTime;
    private string currentItem;

    [Header("Soda Boost Settings")]
    [SerializeField] private Transform sodaSplashPosition;
    private bool isCarryingSoda;
    [SerializeField] private float boostForce;
    [SerializeField] private float boostDuration;
    private bool isDashing;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerAnimatorManager pam;
    private VFXPool vfxpool;
    private ItemDisplay itemDisplay;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        pam = GetComponent<PlayerAnimatorManager>();
        vfxpool = GameObject.FindGameObjectWithTag("VFXPoolManager").GetComponent<VFXPool>();
        itemDisplay = GameObject.FindGameObjectWithTag("ItemDisplay").GetComponent<ItemDisplay>();

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

    #region Player Controls
    private void PlayerInputs()
    {
        if (isDashing) return;

        movement = Input.GetAxisRaw("Horizontal") * (!isSlowed ? currentSpeed : currentSpeed / slowDivider);

        Jump();
        Dash();
        UseItem();
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

    #region Dash

    private void Dash()
    {
        if (isCarryingSoda && Input.GetButtonDown("Fire3"))
        {
            isCarryingSoda = false;
            SpawnSodaSplash(sodaSplashPosition);
            rb.gravityScale = 0;
            isDashing = true;
            rb.linearVelocity = new(0, 0);
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
        rb.gravityScale = 1;
        isDashing = false;
    }

    #endregion

    #region Jump
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
            SpawnSodaSplash(180);
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

    #endregion

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            switch (currentItem)
            {
                case "Beer":
                    StartCoroutine(ActivateBeer());
                    break;
                case "Sugar":
                    StartCoroutine(ActivateSugar());
                    break;
                case "Soda":
                    isCarryingSoda = true;
                    break;
                default:
                    break;
            }

            currentItem = itemDisplay.ClearItem();
        }
    }

    #endregion

    #region Getters
    
    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetSugarMultiplier()
    {
        return sugarSpeedMultiplier;
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

    #endregion

    #region Power-Ups
    private IEnumerator ActivateBeer()
    {
        if (currentBeerTime == 0)
        {
            currentBeerTime = timedPowerUpMaxTime;

            Time.timeScale = beerTimeScale;

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

            currentSpeed = baseSpeed * sugarSpeedMultiplier;

            while (currentSugarTime > 0)
            {
                currentSugarTime--;
                yield return new WaitForSecondsRealtime(1.0f);
            }

            currentSpeed = baseSpeed;
        }
    }

    private void SpawnSodaSplash(float rotation)
    {
        vfxpool.GetPooledSodaSplash(sodaSplashPosition.position, rotation);
    }

    private void SpawnSodaSplash(Transform parent)
    {
        vfxpool.GetPooledSodaSplash(parent);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SewerWater"))
        {
            isSlowed = true;
        } 
        else if (collision.CompareTag("BeerPowerUp"))
        {
            currentItem = itemDisplay.DisplayItem("Beer");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SugarPowerUp"))
        {
            currentItem = itemDisplay.DisplayItem("Sugar");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SodaPowerUp"))
        {
            currentItem = itemDisplay.DisplayItem("Soda");

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
