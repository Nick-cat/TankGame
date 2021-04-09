using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColliderData : MonoBehaviour
{
    private TankComponentManager tcm;

    public static event Action<Collision> OnCollision = collisionPosition => { };

    private void Awake()
    {
        tcm = GetComponent<TankComponentManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        OnCollision(other);
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 surfaceNormalSum = Vector3.zero;
        for (int i = 0; i < collision.contactCount; i++)
        {
            surfaceNormalSum += collision.contacts[i].normal;
        }
        tcm.averageColliderSurfaceNormal = surfaceNormalSum.normalized;
    }

    private void OnCollisionExit(Collision collision)
    {
        tcm.averageColliderSurfaceNormal = Vector3.zero;
    }
}
