using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAcceleration : MonoBehaviour
{
    [SerializeField] float accelerationTime = 0.05f;
    
    private float oldAcceleration;
    private float gas;
    [HideInInspector] public float acceleration;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gas = Input.GetAxisRaw("Vertical");
        Acceleration();
        if (rb.velocity.magnitude < 1) accelerationTime = 1f;
        else accelerationTime = 0.05f;
    }

    void Acceleration()
    {
        oldAcceleration = acceleration;
        acceleration = Mathf.MoveTowards(oldAcceleration, gas, accelerationTime * Time.deltaTime);
    }
}
