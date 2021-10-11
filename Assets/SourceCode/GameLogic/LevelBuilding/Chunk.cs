using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Min(0)] public int appearsOnDifficulty = 0;
    [HideInInspector] public float height { get; private set; }
    [HideInInspector] public GameObject mirrorClone;

    [SerializeField] private GameObject topPoint;
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
        maxY = TopPoint.transform.position.y;
        minY = bottomPoint.transform.position.y;
        height = maxY - minY;
    }

    public float GetDistanceFromCenterToTop()
    {
        return TopPoint.transform.position.y - transform.position.y;
    }

    public float GetDistanceFromCenterToBottom()
    {
        return transform.position.y - bottomPoint.transform.position.y;
    }

    public float GetHeight()
    {
        return GetDistanceFromCenterToTop() + GetDistanceFromCenterToBottom();
    }

    private void OnDestroy()
    {
        if (mirrorClone != null)
        {
            Destroy(mirrorClone);
        }
    }
}
