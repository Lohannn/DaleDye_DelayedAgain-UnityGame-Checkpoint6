using System.Collections;
using Unity.VisualScripting;
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
    private Vector2 respawnPoint = new Vector2(4.94f, 9.54f);
    private bool isDead;
    private int lifes = 3;
    private bool startSceneStarted;
    private bool onStartScene = true;
    private Vector2 startSceneTarget = new Vector2(5, 9);

    [Header("Sensor Ground Settings")]
    [SerializeField] private Transform groundSensor;
    [SerializeField] private Vector2 groundSensorSize;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask PlatformLayer;
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
    private PlayerAudioPlayer pap;
    private VFXPool vfxpool;
    private ItemDisplay itemDisplay;
    private AnxietyTimer timer;
    private MainCanvas mainCanvas;
    private MusicPlayer musicPlayer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        pam = GetComponent<PlayerAnimatorManager>();
        pap = GetComponent<PlayerAudioPlayer>();
        vfxpool = GameObject.FindGameObjectWithTag("VFXPoolManager").GetComponent<VFXPool>();
        itemDisplay = GameObject.FindGameObjectWithTag("ItemDisplay").GetComponent<ItemDisplay>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<AnxietyTimer>();
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvas>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicPlayer>();

        currentSpeed = baseSpeed;

        StartCoroutine(StartSceneCoroutine());
        StartCoroutine(StartAlarmSound());
    }

    void Update()
    {
        StartSceneManagement();

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
        if (isDashing || isDead || onStartScene) return;

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
        if (Input.GetButtonDown("Jump") && (OnGround() || OnPlatform()))
        {
            pam.Jump();

            if (Random.Range(0, 5) < 4)
            {
                pap.PlaySound(pap.JUMP);
            }

            currentJumpTime = maxJumpTime;
        }
        else if (Input.GetButtonDown("Jump") && isCarryingSoda && !OnGround() && !OnPlatform())
        {
            pap.PlaySound(pap.JUMP);
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
                    pap.PlaySound(pap.USE_SODA);
                    isCarryingSoda = true;
                    break;
                default:
                    break;
            }

            currentItem = itemDisplay.ClearItem();
        }
    }

    #endregion

    private IEnumerator DeathCoroutine()
    {
        if (lifes > 0)
        {
            lifes--;
        }

        isDead = true;
        currentJumpTime = 0;
        rb.gravityScale = 0;
        movement = 0;
        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        for (int i = 0; i < 10; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        if (lifes <= 0)
        {
            mainCanvas.StageLose();
        }
        else
        {
            transform.position = respawnPoint;
            rb.gravityScale = 2;
            isDead = false;
            col.enabled = true;
        }
    }

    #region Getters

    public bool GetOnStartScene()
    {
        return onStartScene;
    }

    public int GetLifes()
    {
        return lifes;
    }

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

    public bool GetIsCarryingSoda()
    {
        return isCarryingSoda;
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
        return Physics2D.OverlapBox(groundSensor.position, groundSensorSize, 0, GroundLayer) && rb.linearVelocityY <= 0.1f && rb.linearVelocityY >= -0.1f;
    }

    public bool OnPlatform()
    {
        return Physics2D.OverlapBox(groundSensor.position, groundSensorSize, 0, PlatformLayer) && rb.linearVelocityY <= 0.1f && rb.linearVelocityY >= -0.1f;
    }

    #endregion

    #region Power-Ups
    private IEnumerator ActivateBeer()
    {
        if (currentBeerTime == 0)
        {
            pap.PlaySound(pap.USE_BEER);
            pap.SlowDownSound();
            musicPlayer.SlowDownSound();
            currentBeerTime = timedPowerUpMaxTime;

            Time.timeScale = beerTimeScale;

            while (currentBeerTime > 0)
            {
                if (Time.timeScale != 0)
                {
                    currentBeerTime--;
                }
                yield return new WaitForSecondsRealtime(1.0f);
            }

            pap.ResetSoundSpeed();
            musicPlayer.ResetSoundSpeed();
            Time.timeScale = 1.0f;
        }
    }

    private IEnumerator ActivateSugar()
    {
        if (currentSugarTime == 0)
        {
            pap.PlaySound(pap.USE_SUGAR);
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
        pap.PlaySound(pap.USE_SODA_BOOST);
        vfxpool.GetPooledSodaSplash(sodaSplashPosition.position, rotation);
    }

    private void SpawnSodaSplash(Transform parent)
    {
        pap.PlaySound(pap.USE_SODA_BOOST);
        vfxpool.GetPooledSodaSplash(parent);
    }

    #endregion

    #region Start Scene
    private IEnumerator StartSceneCoroutine()
    {
        yield return new WaitForSeconds(7.0f);
        startSceneStarted = true;
        rb.linearVelocity = new Vector2(0, -1);
    }

    private IEnumerator StartAlarmSound()
    {
        yield return new WaitForSeconds(3.0f);
        pap.PlaySound(pap.ALARM);

        StartCoroutine(StopAlarmSound());
    }

    private IEnumerator StopAlarmSound()
    {
        yield return new WaitForSeconds(4.0f);
        pap.StopSound();
    }

    private void StartSceneManagement()
    {
        if (startSceneStarted)
        {
            if (onStartScene)
            {
                transform.position = Vector2.MoveTowards(transform.position, startSceneTarget, (baseSpeed * 2f) * Time.deltaTime);
            }

            if (onStartScene && !col.enabled && transform.position.x >= 0.5)
            {
                pap.PlaySound(pap.BREAK_GLASS);
                musicPlayer.PlaySound(musicPlayer.STAGE);
                col.enabled = true;
            }

            if (onStartScene && Vector2.Distance(transform.position, startSceneTarget) <= 0.1f)
            {
                rb.gravityScale = 2;
                onStartScene = false;
                StartCoroutine(timer.StartTimer());
            }
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            if (OnPlatform() && collision != null)
            {
                transform.SetParent(collision.transform);
            }
        }
        else if (collision.gameObject.CompareTag("Sign"))
        {
            if (transform.parent != null && transform.parent.transform.CompareTag("Car"))
            {
                StartCoroutine(DeathCoroutine());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            transform.SetParent(null);
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
            pap.PlaySound(pap.PICK_ITEM);
            currentItem = itemDisplay.DisplayItem("Beer");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SugarPowerUp"))
        {
            pap.PlaySound(pap.PICK_ITEM);
            currentItem = itemDisplay.DisplayItem("Sugar");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SodaPowerUp"))
        {
            pap.PlaySound(pap.PICK_ITEM);
            currentItem = itemDisplay.DisplayItem("Soda");

            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Vehicle"))
        {
            pap.PlaySound(pap.DEATH);
            StartCoroutine(DeathCoroutine());
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            respawnPoint = collision.transform.position;
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Objective"))
        {
            mainCanvas.StageWin();
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
