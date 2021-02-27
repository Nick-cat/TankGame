using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float padForce;
    public Vector3 launchDirection; 
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.root.GetComponent<Rigidbody>().AddForce(launchDirection * padForce * 100f);
    }
}
