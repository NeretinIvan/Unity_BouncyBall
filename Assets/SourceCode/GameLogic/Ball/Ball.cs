using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Tooltip("Y coordinate, below which considered as fail")]
    public float deathHeightY;
    [HideInInspector()] public bool chargingAllowed;
    [HideInInspector()] public bool gameOver;
    private Rigidbody2D ballRigidbody;
    [SerializeField] private GameObject progressControllerObject;

    GameLogicController gameLogicController;
    ColorController colorController;

    private Vector3 startingPosition;
    private float maxHeight;

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        gameLogicController = FindObjectOfType<GameLogicController>();
        colorController = FindObjectOfType<ColorController>();
        Initialize();

        gameLogicController.OnGameStarted += GameLogicController_OnGameStarted;
        colorController.OnPaletteChange += Ball_OnPaletteChange;
    }

    private void Ball_OnPaletteChange(object sender, ColorController.OnPaletteChangeEventArgs e)
    {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startColor = e.palette.ballTraceColor;
        trailRenderer.endColor = e.palette.ballTraceColor;
    }

    private void GoToStart()
    {
        transform.position = startingPosition;
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0;
    }

    public void Initialize()
    {
        chargingAllowed = true;
        gameOver = false;
        startingPosition = transform.position;
        maxHeight = startingPosition.y;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        chargingAllowed = true;
    }

    private void Update()
    {
        if ((transform.position.y <= deathHeightY) && (!gameOver))
        {            
            gameOver = true;
            chargingAllowed = false;
            BallFailed();
        }

        if (transform.position.y > maxHeight)
        {
            maxHeight = transform.position.y;
            gameLogicController.SetScore(Mathf.CeilToInt(maxHeight - startingPosition.y));
        }
    }

    public void BallFailed()
    {
        gameOver = true;
        chargingAllowed = false;
        gameLogicController.GameOver();
    }

    public void ChargeBall(Vector3 direction, float force)
    {
        if (!chargingAllowed) return;
        chargingAllowed = false;
        direction = Vector3.Normalize(direction);
        ballRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        gameLogicController.OnGameStarted -= GameLogicController_OnGameStarted;
        colorController.OnPaletteChange -= Ball_OnPaletteChange;
    }

    private void GameLogicController_OnGameStarted(object sender, System.EventArgs e)
    {
        GoToStart();
        Initialize();
    }
}
