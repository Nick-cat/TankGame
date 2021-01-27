using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float tankSpeed = 5f;
    public float rotateSpeed = 5f;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        float forwards = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        // Move tank forward/backwards
        Vector3 moveTank = transform.position + (transform.forward * forwards * tankSpeed * Time.deltaTime);
        rb.MovePosition(moveTank);

        // Rotate tank
        Quaternion rotateTank = transform.rotation * Quaternion.Euler(Vector3.up * (rotateSpeed * rotate * Time.deltaTime));
        rb.MoveRotation(rotateTank);

    }
}
