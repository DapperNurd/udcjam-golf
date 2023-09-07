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
    bool isAiming;

    // Start is called before the first frame update
    void Start()
    {
        ballMovementScript = ball.GetComponent<BallMovement>();
        launchVel = Vector2.zero;
        isAiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(!isAiming) ballMovementScript.Reset();
            isAiming = true;
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        else if(Input.GetMouseButton(0)) {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currentDrag = launchVel + dragEndPos - dragStartPos;
            trajectory.SimulateTrajectory(ball, ball.transform.position, currentDrag);
        }

        else if(Input.GetMouseButtonUp(0)) {
            launchVel = launchVel + dragEndPos - dragStartPos;
        }

        else if(Input.GetMouseButtonDown(1) && isAiming) {
            ballMovementScript.Hit(launchVel);
            launchVel = Vector2.zero;
            isAiming = false;
        }
    }
}
