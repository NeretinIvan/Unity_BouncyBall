using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class TraceDrawer : MonoBehaviour
{
    [Tooltip("Amount of prediction iterations. This will increase length of trace at cost of performance")]
    [SerializeField] [Min(0)] private int maxIterations = 50;
    [Tooltip("Increasing will make trace longer at cost of lane accuracy. Performance will not be decreased")]
    [SerializeField] [Min(1)] private float stepKoef = 1;

    private GameObject tracingObject;
    private GameObject obstaclesRoot;
    private GameObject simulatingObject;
    private Rigidbody2D tracingObjectRigidbody;

    private Scene currentScene;
    private PhysicsScene2D currentPhysicsScene;
    private Scene predictionScene;
    private PhysicsScene2D predictionPhysicsScene;
    private LineRenderer traceLine;

    void Awake()
    {
        traceLine = GetComponent<LineRenderer>();
        obstaclesRoot = FindObjectOfType<ObstaclesRoot>().gameObject;
        tracingObject = FindObjectOfType<Ball>().gameObject;
        tracingObjectRigidbody = tracingObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("predictionScene", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();
        Physics2D.autoSimulation = false;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D();

        ClearTrace();
        RefreshObstacles();
    }

    //Don't remove this
    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }
    
    public void MakePrediction(Vector3 direction, float force)
    {
        if (simulatingObject == null)
        {
            simulatingObject = Instantiate(tracingObject);           
        }
        SceneManager.MoveGameObjectToScene(simulatingObject, predictionScene);
        simulatingObject.transform.position = tracingObject.transform.position;
        simulatingObject.GetComponent<Rigidbody2D>().velocity = tracingObjectRigidbody.velocity;
        simulatingObject.GetComponent<Rigidbody2D>().angularVelocity = tracingObjectRigidbody.angularVelocity;

        Ball ballComponent = simulatingObject.GetComponent<Ball>();
        ballComponent.chargingAllowed = true;
        ballComponent.ChargeBall(direction, force);
        
        traceLine.positionCount = maxIterations;
        for (int i = 0; i < maxIterations; i++)
        {
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime * stepKoef);
            traceLine.SetPosition(i, simulatingObject.transform.position);
        }

        Destroy(simulatingObject);
    }

    public void ClearTrace()
    {
        traceLine.positionCount = 0;
    }

    public void RefreshObstacles()
    {
        DestroyObstaclesInPredictionScene();
        foreach (Transform child in obstaclesRoot.transform)
        {
            GameObject levelObject = Instantiate(child.gameObject);
            SceneManager.MoveGameObjectToScene(levelObject, predictionScene);
            levelObject.transform.position = child.position;
        }
    }

    private void DestroyObstaclesInPredictionScene()
    {
        GameObject[] rootObjects = predictionScene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; i++)
        {
            Destroy(rootObjects[i]);
        }
    }
}
