using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallBuilder : MonoBehaviour
{
    [SerializeField()] private float destroyChunkRange;
    [SerializeField()] private float buildChunkRange;
    [SerializeField()] private GameObject chunksStorage;
    [SerializeField()] private GameObject firstChunk;
    [SerializeField()] private GameObject obstaclesRoot;

    public event EventHandler<ChunkCreatedEventArgs> OnChunkCreated;
    public class ChunkCreatedEventArgs
    {
        public Chunk createdChunk { get; set; }
    }

    private TraceDrawer traceDrawer;
    private GameObject ball;
    private Vector3 startingBallPosition;
    private List<Chunk> chunksLoaded;

    private void Awake()
    {
        traceDrawer = FindObjectOfType<TraceDrawer>();
        ball = FindObjectOfType<Ball>().gameObject;
        FindObjectOfType<GameLogicController>().OnGameStarted += WallBuilder_OnGameStarted;

        chunksLoaded = new List<Chunk>();
        startingBallPosition = ball.transform.position;
        OnChunkCreated += WallBuilder_OnChunkCreated;
    }

    private void WallBuilder_OnChunkCreated(object sender, ChunkCreatedEventArgs e)
    {
        chunksLoaded.Add(e.createdChunk);
    }

    void Update()
    {
        CheckBuildingChunks();
        CheckDisappearingChunks();
    }

    private void BuildChunk(Chunk chunkPrototype, float Ycoord)
    {
        GameObject chunkCreated = Instantiate(chunkPrototype.gameObject);
        chunkCreated.transform.position = new Vector3(startingBallPosition.x, Ycoord, startingBallPosition.z);
        chunkCreated.transform.SetParent(obstaclesRoot.transform);
        chunkCreated.SetActive(true);
        OnChunkCreated.Invoke(this, new ChunkCreatedEventArgs { createdChunk = chunkCreated.GetComponent<Chunk>() });
    }

    private void CheckDisappearingChunks()
    {
        if (chunksLoaded.Count <= 1) return;

        Chunk removeChunk = null;
        foreach(Chunk chunk in chunksLoaded)
        {
            if ((ball.transform.position.y - chunk.TopPoint.transform.position.y >= destroyChunkRange) ||
                (chunk.BottomPoint.transform.position.y - ball.transform.position.y >= destroyChunkRange))
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

        Chunk latestChunk = chunksLoaded[chunksLoaded.Count - 1];
        if (latestChunk.TopPoint.transform.position.y - ball.transform.position.y <= buildChunkRange)
        {
            Chunk prototype = PickChunk();
            BuildChunk(prototype, latestChunk.TopPoint.transform.position.y + prototype.GetDistanceFromCenterToBottom());
        }
    }

    private Chunk PickChunk()
    {
        int chunkNumber = UnityEngine.Random.Range(0, chunksStorage.transform.childCount);
        Chunk pickedChunk = chunksStorage.transform.GetChild(chunkNumber).GetComponent<Chunk>();
        return pickedChunk;
    }

    private void DestroyAllChunks()
    {
        Chunk[] chunks = obstaclesRoot.transform.GetComponentsInChildren<Chunk>();
        for (int i = 0; i < chunks.Length; i++)
        {
            Destroy(chunks[i].gameObject);
        }
        chunksLoaded = new List<Chunk>();
    }

    private void WallBuilder_OnGameStarted(object sender, System.EventArgs e)
    {
        DestroyAllChunks();
        BuildChunk(firstChunk.GetComponent<Chunk>(), startingBallPosition.y);
    }
}
