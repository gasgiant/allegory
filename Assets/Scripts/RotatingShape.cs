using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShape : MonoBehaviour {

    public Vector3 axis = Vector3.right;
    public float angularVelocity = 180;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(axis * angularVelocity * Time.fixedDeltaTime) * rb.rotation );
    }
}
