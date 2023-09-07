using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BallMovement : MonoBehaviour
{

    Rigidbody2D rb;
    CircleCollider2D col;

    [SerializeField] float hitPower;

    Vector2 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            // transform.position = new Vector3(0, -14.5f, 0);
            // transform.rotation = Quaternion.identity;
            // rb.velocity = Vector2.zero;
            // rb.angularVelocity = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Goal")
            StartCoroutine(CheckWin(1f));
    }

    public void Hit(Vector2 vel) {
        rb.angularVelocity = 0;
        rb.velocity = vel * hitPower;
    }

    public void Reset() {
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
        transform.position = startPos;
    }

    void PlayerWon() {
        Debug.Log("Player won");
    }

    IEnumerator CheckWin(float seconds) {
        yield return new WaitForSeconds(seconds);
        if(col.IsTouchingLayers(LayerMask.GetMask("Goal"))) // Returns whether it is or isn't a second later to make sure it stayed in the hole
            PlayerWon();
    }
}
