using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charger : MonoBehaviour
{
    [SerializeField] private InputHandler.InputType inputType;
    [SerializeField] [Min(0)] private float strengthKoef = 0.001f;
    [SerializeField] [Min(0)] private float maxStrength = 100f;
    [SerializeField] [Range(0, 1)] private float slowTimeKoef = 1f;

    private InputHandler inputHandler;
    private Ball ballComponent;
    private TraceDrawer traceDrawer;
    private ChargeLineDrawer chargeLineDrawer;

    private bool charging = false;
    private Vector2 startingPoint;
    private float strength;
    private Vector3 direction;

    private void Awake()
    {
        if (inputType == InputHandler.InputType.touch)
        {
            inputHandler = new InputHandler_touch();
        }
        else
        {
            inputHandler = new InputHandler_mouse();
        }
        ballComponent = FindObjectOfType<Ball>();
        traceDrawer = GetComponentInChildren<TraceDrawer>();
        chargeLineDrawer = GetComponentInChildren<ChargeLineDrawer>();
    }

    private void ChargingStarted(Vector2 touchPosition)
    {
        charging = true;
        startingPoint = touchPosition;
        strength = 0;
        direction = Vector3.zero;
        chargeLineDrawer.Activate();
        FindObjectOfType<PauseController>().SlowTime(slowTimeKoef);
    }

    private void ChargingEnded(Vector2 touchPosition)
    {
        InterruptCharging();
        ballComponent.ChargeBall(direction, strength);
    }

    public void InterruptCharging()
    {
        charging = false;
        traceDrawer.ClearTrace();
        chargeLineDrawer.Deactivate();
        FindObjectOfType<PauseController>().RemoveSlowTime();
    }

    private void OnCharging(Vector2 touchPosition)
    {
        float chargeDistance = Vector2.Distance(startingPoint, touchPosition);
        strength = Mathf.Min(chargeDistance, maxStrength) * strengthKoef;
        direction = startingPoint - touchPosition;
        traceDrawer.MakePrediction(direction, strength);

        if (chargeDistance > maxStrength)
        {
            Vector3 cuttedVector = Vector3.Normalize(touchPosition - startingPoint) * maxStrength;
            touchPosition = startingPoint + new Vector2(cuttedVector.x, cuttedVector.y);
        }   
        chargeLineDrawer.SetLinePositions(startingPoint, touchPosition);
    }

    private void CheckInput()
    {
        if (!ballComponent.chargingAllowed)
        {
            InterruptCharging();
            return;
        }

        if (inputHandler.InputStarted())
        {
            ChargingStarted(inputHandler.GetInputPosition());
        }
        if (inputHandler.InputEnded())
        {
            ChargingEnded(inputHandler.GetInputPosition());
        }
    }

    void Update()
    {
        CheckInput();

        if (charging)
        {
            OnCharging(inputHandler.GetInputPosition());
        }
    }
}
