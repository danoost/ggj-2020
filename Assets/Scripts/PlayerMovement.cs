using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 400f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Debug.Log(InputHandler.MovementDirection);
        rb.velocity = InputHandler.MovementDirection * speed * Time.fixedDeltaTime;
    }
}
