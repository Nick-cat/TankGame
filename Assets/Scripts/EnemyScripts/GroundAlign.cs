using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAlign : MonoBehaviour
{
    private LayerMask ground;
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Align();
    }

    void Align()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 20, ground))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.parent.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 15 * Time.deltaTime);
        }
    }
}
