using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private GameObject player;
    public bool lookAtPlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }
    private void Update()
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
