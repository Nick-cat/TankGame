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

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 20, ground))
        {
            transform.up = Vector3.Lerp(transform.up, hit.normal, 15 * Time.deltaTime);
        }
    }
}
