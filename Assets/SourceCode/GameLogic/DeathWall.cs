using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [Min(0)] [SerializeField()] private float maxDistanceFromCamera = 1;
    [Tooltip("Time, in which wall stands still before move on, in seconds")]
    [Min(0)] [SerializeField()] private float cooldownTime = 2;
    [Min(0)] [SerializeField()] private float baseSpeed = 2;
    [Min(0)] [SerializeField()] private float maxSpeed = 2;
    [Tooltip("Each new level of difficulty speed of wall will increase by this number")]
    [Min(0)] [SerializeField] private float speedRiseValue;

    private Vector3 startingPosition;
    private float cooldownRemaining;
    private float currentSpeed;

    private void Awake()
    {
        Vector3 ballPosition = FindObjectOfType<Ball>().transform.position;
        startingPosition = new Vector3(ballPosition.x, transform.position.y, transform.position.z);
        currentSpeed = baseSpeed;

        FindObjectOfType<GameLogicController>().OnGameStarted += DeathWall_OnGameStarted;
        FindObjectOfType<GameLogicController>().OnDifficultyUpdate += DeathWall_OnDifficultyUpdate;
        FindObjectOfType<ColorController>().OnPaletteChange += DeathWall_OnPaletteChange;
        gameObject.SetActive(false);
    }

    private void DeathWall_OnPaletteChange(object sender, ColorController.OnPaletteChangeEventArgs e)
    {
        Material deathWallMaterial = GetComponent<MeshRenderer>().material;
        deathWallMaterial.color = e.palette.lavaColor;
        //Light deathWallLight = GetComponentInChildren<Light>();
        //deathWallLight.color = e.palette.lavaColor;
    }

    private void DeathWall_OnDifficultyUpdate(object sender, GameLogicController.DifficultyUpdateEventArgs e)
    {
        currentSpeed = Mathf.Min(baseSpeed + speedRiseValue * (e.Difficulty - 1), maxSpeed);
    }

    private void DeathWall_OnGameStarted(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        transform.position = startingPosition;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            ball.BallFailed();
        }
    }

    void Update()
    {
        cooldownRemaining = Mathf.Max(cooldownRemaining - Time.deltaTime, 0);
        TryMove();
        SyncWithCamera();
    }

    private void ResetCooldown()
    {
        cooldownRemaining = cooldownTime;
    }

    private void SyncWithCamera()
    {
        if (transform.position.y + maxDistanceFromCamera < Camera.main.transform.position.y)
        {
            transform.position = new Vector3(
                transform.position.x,
                Camera.main.transform.position.y - maxDistanceFromCamera,
                transform.position.z);
            ResetCooldown();
        }
    }

    private void TryMove()
    {
        if (cooldownRemaining <= 0)
        {
            transform.position = new Vector3(
            transform.position.x,
            transform.position.y + currentSpeed * Time.deltaTime,
            transform.position.z);
        }       
    }
}
