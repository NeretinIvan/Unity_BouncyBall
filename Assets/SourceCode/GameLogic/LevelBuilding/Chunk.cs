using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector()] public float height { get; private set; }
    
    [SerializeField()] private GameObject topPoint;
    public GameObject TopPoint
    {
        get { return topPoint; }
        private set { topPoint = value; }
    }

    [SerializeField()] private GameObject bottomPoint;
    public GameObject BottomPoint 
    {
        get { return bottomPoint; }
        private set { bottomPoint = value; }
    }

    private float maxY;
    private float minY;

    private void Awake()
    {
        if ((TopPoint != null) && (bottomPoint != null))
        {
            maxY = TopPoint.transform.position.y;
            minY = bottomPoint.transform.position.y;
        }
        else
        {
            FindDimensionsAutomaticly();            
        }
        height = maxY - minY;
    }

    private void FindDimensionsAutomaticly()
    {
        if (transform.childCount <= 0) return;

        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach(Transform child in transform)
        {
            float bottomPointY = child.position.y - child.localScale.y / 2;
            float topPointY = child.position.y + child.localScale.y / 2;
            if (bottomPointY < minY)
            {
                minY = bottomPointY;
            }
            if (topPointY > maxY)
            {
                maxY = topPointY;
            }
        }

        this.maxY = maxY;
        this.minY = minY;
    }

    public float GetDistanceFromCenterToTop()
    {
        return TopPoint.transform.position.y - transform.position.y;
    }

    public float GetDistanceFromCenterToBottom()
    {
        return transform.position.y - bottomPoint.transform.position.y;
    }
}
