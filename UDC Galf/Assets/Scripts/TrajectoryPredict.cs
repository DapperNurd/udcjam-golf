using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TrajectoryPredict : MonoBehaviour
{
 
    Scene simulationScene;
    PhysicsScene2D physicsScene;

    [SerializeField] GameObject tileGrid;
    [SerializeField] Transform physicsAffectorsParent;

    [SerializeField] LineRenderer currentTrajectoryLine;
    [SerializeField] LineRenderer previousTrajectoryLine;
    [Range(1, 300)] [SerializeField] int totalCalculatedFrames = 150;
    [Range(1, 20)] [SerializeField] int framesPerCalculation = 1;

    void Start() {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene() {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        var ghostGrid = Instantiate(tileGrid, tileGrid.transform.position, tileGrid.transform.rotation);
        ghostGrid.GetComponentInChildren<TilemapRenderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostGrid, simulationScene);

        foreach(Transform obj in physicsAffectorsParent) {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponentInChildren<SpriteRenderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    public void SimulateTrajectory(GameObject ball, Vector2 pos, Vector2 velocity) {
        var ghostObj = Instantiate(ball, pos, Quaternion.identity);
        ghostObj.GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);

        int positionCount = totalCalculatedFrames / framesPerCalculation;
        currentTrajectoryLine.positionCount = positionCount;
        currentTrajectoryLine.SetPosition(0, ghostObj.transform.position); // Manually set the first position before shooting the ball to keep the line renderer starting at the ball's position
        ghostObj.GetComponent<BallMovement>().Hit(velocity);
        for(int i = 1; i < positionCount; i++) {
            physicsScene.Simulate(Time.fixedDeltaTime * framesPerCalculation);
            currentTrajectoryLine.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj);
        physicsScene.Simulate(Time.fixedDeltaTime);
    }

    public void ResetTrajectory() {        
        Vector3[] currentTrajectory = new Vector3[currentTrajectoryLine.positionCount];
        currentTrajectoryLine.GetPositions(currentTrajectory);

        previousTrajectoryLine.positionCount = currentTrajectoryLine.positionCount;
        previousTrajectoryLine.SetPositions(currentTrajectory);

        currentTrajectoryLine.positionCount = 0;
    }

    public void ClearPreviousTrajectory() {
        previousTrajectoryLine.positionCount = 0;
    }

    public void SetColor(Color color) {
        var gradient = new Gradient();
        var colors = new GradientColorKey[1];
        var alphas = new GradientAlphaKey[3];

        alphas[0] = new GradientAlphaKey(0.7f, 0.0f);
        alphas[1] = new GradientAlphaKey(0.7f, 0.9f);
        alphas[2] = new GradientAlphaKey(0.0f, 1.0f);

        colors[0] = new GradientColorKey(color, 0.0f);

        gradient.SetKeys(colors, alphas);

        currentTrajectoryLine.colorGradient = gradient;
    }
 
}