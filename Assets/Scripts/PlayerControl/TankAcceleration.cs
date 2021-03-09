using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAcceleration : MonoBehaviour
{
    [SerializeField] float accelerationTime = 0.05f;
    
    private float oldAcceleration;
    private float gas;
    [HideInInspector] public float acceleration;

    // Update is called once per frame
    void FixedUpdate()
    {
        gas = Input.GetAxisRaw("Vertical");
        Acceleration();
    }

    void Acceleration()
    {
        oldAcceleration = acceleration;
        acceleration = Mathf.MoveTowards(oldAcceleration, gas, accelerationTime * Time.deltaTime);
    }
}
