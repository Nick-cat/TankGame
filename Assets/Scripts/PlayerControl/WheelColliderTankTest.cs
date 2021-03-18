using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelColliderTankTest : MonoBehaviour
{
    [SerializeField]List<WheelCollider> turningWheels;
    [SerializeField] List<WheelCollider> drivingWheels;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float forwards = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        foreach (WheelCollider wheel in turningWheels) 
        { 
            wheel.steerAngle = steer * turnSpeed;
            wheel.motorTorque = forwards * speed * Time.deltaTime * 100f;
        }
        foreach (WheelCollider wheel in drivingWheels)
        {
            wheel.motorTorque = forwards * speed * Time.deltaTime * 100f;
        }
    }
}
