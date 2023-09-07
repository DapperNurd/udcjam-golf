using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBall : MonoBehaviour
{
    [SerializeField] GameObject ball;

    BallMovement ballMovementScript;
    Rigidbody2D rb;
    LineRenderer lr;

    [SerializeField] TrajectoryPredict trajectory;
    Vector2 dragStartPos, dragEndPos, launchVel;

    // Start is called before the first frame update
    void Start()
    {
        ballMovementScript = ball.GetComponent<BallMovement>();
        launchVel = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0)) {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currentDrag = launchVel + dragEndPos - dragStartPos;
            trajectory.SimulateTrajectory(ball, ball.transform.position, currentDrag);
        }

        if(Input.GetMouseButtonUp(0)) {
            launchVel = launchVel + dragEndPos - dragStartPos;
        }

        if(Input.GetMouseButtonDown(1) && !ballMovementScript.IsMoving()) {
            ballMovementScript.Hit(launchVel);
            launchVel = Vector2.zero;
        }
    }
}
