using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    private void Awake()
    {
        if(waypoints.Length == 0) waypoints = GameObject.FindGameObjectsWithTag("WayPoint");
    }

    public Vector3 GetNextTarget()
    {
        currentWP++;
        if (currentWP >= waypoints.Length) currentWP = 0;
        return waypoints[currentWP].transform.position;
    }
}
