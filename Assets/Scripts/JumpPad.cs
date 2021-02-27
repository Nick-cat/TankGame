using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float padForce;
    public Vector3 launchDirection;


    void OnTriggerEnter(Collider other)
    {
            other.gameObject.transform.root.GetComponent<Rigidbody>()
                .AddForce(transform.InverseTransformDirection(launchDirection) * padForce * 100f);
    
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(launchDirection) * 2f, Color.green);
    }
}
