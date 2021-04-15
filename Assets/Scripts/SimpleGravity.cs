using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGravity : MonoBehaviour
{
    private Rigidbody rb;
    public float customGravity = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(Vector3.up * -customGravity * 100f);
    }
}
