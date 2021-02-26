using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position = Camera.main.transform.position;

    }

}
