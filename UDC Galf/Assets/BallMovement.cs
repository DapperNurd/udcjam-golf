using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class BallMovement : MonoBehaviour
{

    Rigidbody2D rb;
    LineRenderer lr;

    [SerializeField] bool isAiming;
    [SerializeField] float hitPower;

    Vector2 dragStartPos, dragEndPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isAiming = true;

            transform.position = new Vector3(0, -14.5f, 0);
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        if(Input.GetMouseButton(0)) {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 hitVelocity = (dragEndPos - dragStartPos) * hitPower;

            Vector2[] trajectory = PlotTrajectory(rb, (Vector2)transform.position, hitVelocity, 250, 2);
            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for(int i = 0; i < trajectory.Length; i++) {
                positions[i] = trajectory[i];
            }
            lr.SetPositions(positions);
        }

        else if(Input.GetMouseButtonUp(0)) {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 hitVelocity = (dragEndPos - dragStartPos) * hitPower;

            isAiming = false;

            rb.angularVelocity = 0;
            rb.velocity = hitVelocity;
        }
    }

    // Tutorial for function: https://www.youtube.com/watch?v=RpeRnlLgmv8
    /// <summary>Calculate the trajectory of a rigidbody using a given velocity.</summary>
    /// <param name="rigidbody">The rigidbody of the object you wish to calculate with.</param>
    /// <param name="pos">Position to calculate trajectory from.</param>
    /// <param name="velocity">Velocity to calculate trajectory with.</param>
    /// <param name="steps">How many steps to calculate (how far the trajectory will predict).</param>
    /// <param name="stepDistance">Accuracy of each calculated step (higher is less accurate, more performant).</param>
    /// <returns>An array of caculated Vector2 positons along a path.</returns>
    public Vector2[] PlotTrajectory(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps, float stepDistance) {
        bool calculateBounce = false;
        int bounceSteps = steps;
        Vector2 surfaceDirection = Vector2.zero;

        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations * stepDistance;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
        
        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for(int i = 0; i < steps; i++) {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
            RaycastHit2D hit = Physics2D.Raycast(pos, moveStep.normalized, 0.4f);
            //if(Physics2D.OverlapCircleAll(pos, 0.1f, 1).Length > 0) {
            if(hit.collider != null) {
                Debug.Log(hit.normal);
                surfaceDirection = hit.normal;
                calculateBounce = true;
                bounceSteps = steps-i;
                break;
            }
        }

        if(calculateBounce) {
            moveStep = Vector2.Reflect(moveStep, surfaceDirection) * rigidbody.sharedMaterial.bounciness;
            int iterationsStart = steps - bounceSteps; // This is just so its not calculated every loop
            for(int i = 0; i < bounceSteps; i++) {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[iterationsStart + i] = pos;
            }
        }

        return results;
    }
}
