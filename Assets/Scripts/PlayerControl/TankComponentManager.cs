using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankComponentManager : MonoBehaviour
{
    public Transform[] wheels;
    public bool isGrounded;
    public Rigidbody rb;
    public int numberOfGroundedWheels;
    public Vector3 averageColliderSurfaceNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (numberOfGroundedWheels <= 0) isGrounded = false;
    }
}
