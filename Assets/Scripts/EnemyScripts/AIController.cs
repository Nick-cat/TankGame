using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    private int rotateDirection;
    [SerializeField] Vector2 strafeRange;
    [HideInInspector] public float strafeDistance;
    [SerializeField] GameObject turret;
    private bool isStrafe = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        bool dir = (Random.value < 0.5f);
        if (dir == true) rotateDirection = 1;
        else rotateDirection = -1;

        strafeDistance = Random.Range(strafeRange.x, strafeRange.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        LookAtPlayer();

        if (CalculateDistance() <= strafeDistance)
        {
            if (!isStrafe)
            {
                Debug.Log("IsStraffing");
                isStrafe = true;
            }
            Strafe();
            return;
        }

        if (isStrafe)
        {
            Debug.Log("notStrafing");
            isStrafe = false;
        }

        agent.SetDestination(target.transform.position);
    }

    void Strafe()
    {
        Vector3 pathToTarget = target.transform.position - transform.position;
        Vector3 dir = Vector3.Cross(pathToTarget, Vector3.up);
        agent.SetDestination(transform.position + (dir * rotateDirection));
    }

    void LookAtPlayer()
    {
        //Quaternion angleToTarget = Quaternion.LookRotation(target.transform.position - turret.transform.position);
        //turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, angleToTarget, 10f * Time.deltaTime);
        turret.transform.LookAt(target.transform);
    }

    public float CalculateDistance()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance;
    }
}
