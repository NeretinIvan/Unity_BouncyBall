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
    //private ProgressController progressController;

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        //progressController = progressControllerObject.GetComponent<ProgressController>();
        Initialize();
    }

    public void Initialize()
    {
        chargingAllowed = true;
        gameOver = false;
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
    }

    public void BallFailed()
    {
        gameOver = true;
        chargingAllowed = false;
        Debug.Log("ball failed");
        //progressController.AdmitFail();
    }

    public void ChargeBall(Vector3 direction, float force)
    {
        if (!chargingAllowed) return;
        chargingAllowed = false;
        direction = Vector3.Normalize(direction);
        ballRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
