using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Tooltip("Makes the billboard face the player instead of the camera")]
    [SerializeField] bool lookAtPlayer;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if (lookAtPlayer)
        {
            transform.LookAt(player.transform, Vector3.up);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = 0;
            transform.eulerAngles = eulerAngles;

        }
        else
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = 0;
            transform.eulerAngles = eulerAngles;
        }
    }
}
