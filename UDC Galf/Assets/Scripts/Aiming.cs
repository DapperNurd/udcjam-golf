using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] GameObject ball;
    BallMovement ballMovementScript;

    [SerializeField] TrajectoryPredict trajectory;
    Vector2 dragStartPos, dragEndPos, launchVel;
    bool isAiming;

    [Range(1, 200)] [SerializeField] int maxPower;

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
            if(!isAiming) {
                ballMovementScript.Reset();
                trajectory.ResetTrajectory();
            }
            isAiming = true;
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        else if(Input.GetMouseButton(0)) {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currentDrag = launchVel + dragEndPos - dragStartPos;
            if(currentDrag.magnitude > maxPower)
                currentDrag = currentDrag.normalized * maxPower;

            float dragPowerPercent = currentDrag.magnitude / maxPower;

            Color lineColor = new Color(2f-(2f-dragPowerPercent*2), 2f-dragPowerPercent*2, 0f, 1f);
            trajectory.SetColor(lineColor);

            trajectory.SimulateTrajectory(ball, ball.transform.position, currentDrag);
        }

        else if(Input.GetMouseButtonUp(0)) {
            launchVel = launchVel + dragEndPos - dragStartPos;
            if(launchVel.magnitude > 100)
                launchVel = launchVel.normalized * 100;
        }

        else if(Input.GetMouseButtonDown(1) && isAiming) {
            trajectory.ClearPreviousTrajectory();
            ballMovementScript.Hit(launchVel);
            isAiming = false;
        }
    }
}
