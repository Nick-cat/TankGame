using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Tooltip("Makes the billboard face the player instead of the camera")]
    [SerializeField] bool lookAtPlayer;

    private GameObject player;
    private Camera cam;
    private Transform billboard;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        billboard = transform;
    }

    void LateUpdate()
    {
        if (lookAtPlayer)
        {
            billboard.LookAt(player.transform, Vector3.up);
            Vector3 eulerAngles = billboard.eulerAngles;
            eulerAngles.x = 0;
            billboard.eulerAngles = eulerAngles;

        }
        else
        {
            billboard.LookAt(billboard.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            Vector3 eulerAngles = billboard.eulerAngles;
            eulerAngles.x = 0;
            billboard.eulerAngles = eulerAngles;
        }
    }
}
