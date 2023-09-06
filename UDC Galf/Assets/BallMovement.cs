using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    Rigidbody2D rb;

    [SerializeField] float hitPower;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            rb.AddForce(Vector2.up * hitPower, ForceMode2D.Impulse);
        }
    }
}
