using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField()] private float destroyChunkRange;
    [SerializeField()] private float buildChunkRange;
    [SerializeField()] private GameObject chunksStorage;
    [SerializeField()] private GameObject firstChunk;

    private TraceDrawer traceDrawer;
    private ObstaclesRoot obstaclesRoot;
    private GameObject ball;
    private Vector3 startingBallPosition;
    private List<Chunk> chunksLoaded;

    private void Awake()
    {
        traceDrawer = FindObjectOfType<TraceDrawer>();
        ball = FindObjectOfType<Ball>().gameObject;
        obstaclesRoot = FindObjectOfType<ObstaclesRoot>();
        startingBallPosition = ball.transform.position;
        chunksLoaded = new List<Chunk>();
    }

    void Start()
    {
        BuildChunk(firstChunk.GetComponent<Chunk>());
    }

    void Update()
    {
        CheckBuildingChunks();
        CheckDisappearingChunks();
    }

    private void BuildChunk(Chunk chunkPrototype)
    {
        GameObject chunkCreated = Instantiate(chunkPrototype.gameObject);
        if (chunksLoaded.Count == 0)
        {
            chunkCreated.transform.position = ball.transform.position;
        }
        else
        {
            Chunk latestChunk = chunksLoaded[chunksLoaded.Count - 1];
            float addY = latestChunk.GetDistanceFromCenterToTop() + chunkPrototype.GetDistanceFromCenterToBottom();
            chunkCreated.transform.position = new Vector3(
                startingBallPosition.x, 
                latestChunk.transform.position.y + addY, 
                startingBallPosition.z);
        }

        chunksLoaded.Add(chunkCreated.GetComponent<Chunk>());
        chunkCreated.transform.SetParent(obstaclesRoot.transform);
        chunkCreated.SetActive(true);
    }

    private void CheckDisappearingChunks()
    {
        if (chunksLoaded.Count <= 1) return;

        Chunk removeChunk = null;
        foreach(Chunk chunk in chunksLoaded)
        {
            if (ball.transform.position.y - chunk.TopPoint.transform.position.y >= destroyChunkRange)
            {
                removeChunk = chunk;
                break;
            }
        }

        if (removeChunk == null) return;
        
        chunksLoaded.Remove(removeChunk);
        Destroy(removeChunk.gameObject);
    }

    private void CheckBuildingChunks()
    {
        if (chunksLoaded.Count == 0) return;

        if (chunksLoaded[chunksLoaded.Count - 1].TopPoint.transform.position.y - ball.transform.position.y <= buildChunkRange)
        {
            BuildChunk(PickChunk());
            traceDrawer.RefreshObstacles();
        }
    }

    private Chunk PickChunk()
    {
        int chunkNumber = Random.Range(0, chunksStorage.transform.childCount);
        Chunk pickedChunk = chunksStorage.transform.GetChild(chunkNumber).GetComponent<Chunk>();
        return pickedChunk;
    }
}
