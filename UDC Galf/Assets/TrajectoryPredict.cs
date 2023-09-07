using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class TrajectoryPredict : MonoBehaviour
{
 
    Scene simulationScene;
    PhysicsScene2D physicsScene;

    [SerializeField] Transform obstaclesParent;

    [SerializeField] LineRenderer lr;
    [Range(1, 300)] [SerializeField] int totalCalculatedFrames = 150;
    [Range(1, 20)] [SerializeField] int framesPerCalculation = 1;

    void Start() {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene() {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in obstaclesParent) {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<SpriteRenderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    public void SimulateTrajectory(GameObject ball, Vector2 pos, Vector2 velocity) {
        var ghostObj = Instantiate(ball, pos, Quaternion.identity);
        ghostObj.GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        ghostObj.GetComponent<BallMovement>().Hit(velocity);

        lr.positionCount = totalCalculatedFrames / framesPerCalculation;
        for(int i = 0; i < totalCalculatedFrames / framesPerCalculation; i++) {
            physicsScene.Simulate(Time.fixedDeltaTime * framesPerCalculation);
            lr.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj);
        physicsScene.Simulate(Time.fixedDeltaTime);
    }
 
}