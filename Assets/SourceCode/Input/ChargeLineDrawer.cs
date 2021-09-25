using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChargeLineDrawer : MonoBehaviour
{
    [HideInInspector()] public Vector2 startingPoint = Vector2.zero;
    [HideInInspector()] public Vector2 endingPoint = Vector2.zero;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Deactivate();
    }

    public void SetLinePositions(Vector2 startingPoint, Vector2 endingPoint)
    {
        this.startingPoint = Camera.main.ScreenToWorldPoint(startingPoint);
        this.endingPoint = Camera.main.ScreenToWorldPoint(endingPoint);
    }

    public void Activate()
    {
        lineRenderer.positionCount = 2;
    }

    public void Deactivate()
    {
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {      
        if (lineRenderer.positionCount >= 2)
        {
            lineRenderer.SetPosition(0, startingPoint);
            lineRenderer.SetPosition(1, endingPoint);
        }
    }
}
