using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeedRange = new Vector2(10f, 20f);
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] Vector2 turnSpeedRange = new Vector2(0.01f, 0.05f);
    [SerializeField] Vector2 roamingRange = new Vector2(50f, 100f);
    [SerializeField] Vector2 strafeRange = new Vector2(20f, 40f);
    [SerializeField] Transform turret;

    [Header("Materials")]
    [SerializeField] MeshRenderer gemMat;
    [SerializeField] Material idleGlow;
    [SerializeField] Material searchingGlow;
    [SerializeField] Material chaseGlow;

    private Vector3 target;
    private Transform player;
    //private NavMeshAgent agent;
    private NavMeshPath path;
    private Rigidbody rb;
    private float moveSpeed;
    private float turnSpeed;
    private float roamingDistance;
    private float strafeDistance;
    private int rotateDirection;
    private bool searchingForPlayer;

    private void Start()
    {
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        turnSpeed = Random.Range(turnSpeedRange.x, turnSpeedRange.y);
        roamingDistance = Random.Range(roamingRange.x, roamingRange.y);
        strafeDistance = Random.Range(strafeRange.x, strafeRange.y);

        //set random strafe direction
        bool dir = (Random.value < 0.5f);
        if (dir == true) rotateDirection = 1;
        else rotateDirection = -1;

        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();

        GetRandomNavPoint();
        GetPathToTarget(target);
        //agent.SetDestination(target);
    }

    private void GetPathToTarget(Vector3 destination)
    {
        //path.ClearCorners();
        NavMesh.CalculatePath(rb.position, destination, NavMesh.AllAreas, path);

        //string s = "Current path.corners : {";
        //foreach (Vector3 v in path.corners)
        //{
        //    s += " || ";
        //    s += v.ToString();
        //}
        //s += " }";
        //Debug.Log(s);
    }

    private void FixedUpdate()
    {
        GetAITarget();
        LookAtTarget();

        transform.Rotate(0, CalculateAngle() * turnSpeed, 0);

        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 forwards = transform.forward * moveSpeed * Time.deltaTime * 100f;
            rb.AddForce(forwards, ForceMode.Acceleration);
        }

    }

    private void GetAITarget()
    {
        if (CanDetectPlayer())
        {
            if (CanHuntPlayer())
            {
                //Debug.Log($"Will Hunt to : {target}");
                Hunt();
                return;
            }

            //Debug.Log($"Will Search to : {target}");
            Search();
            return;
        }

        //Debug.Log($"Will Roam to : {target}");
        //gets a new random point on the navmesh to move towards if it is close enough to the previous target location
        Roam();
    }

    private bool CanHuntPlayer()
    {
        return CalculateDistance(player.position) < strafeDistance * 5f;
        //if (CalculateDistance(player.transform.position) < strafeDistance * 5f) return true;
        //return false;
    }

    private bool CanDetectPlayer()
    {
        return CalculateDistance(player.position) < strafeDistance * 6f;
    }

    private void Roam()
    {
        gemMat.material = idleGlow;
        //if (agent.remainingDistance < strafeDistance)
        if ((rb.position - target).magnitude < strafeDistance) // Check the distance between self and the target position.
        {
            GetRandomNavPoint();
            GetPathToTarget(target);
        }
    }

    private void Search()
    {
        gemMat.material = searchingGlow;
        if (!searchingForPlayer)
        {
            target = player.position;
            searchingForPlayer = true;
        }
        GetPathToTarget(target);
    }

    private void Hunt()
    {
        if (searchingForPlayer) searchingForPlayer = false;
        gemMat.material = chaseGlow;
        if (CalculateDistance(player.position) <= strafeDistance)
        {
            //makes the AI strafe around the player
            Strafe();
            return;
        }
        target = player.position;
        GetPathToTarget(target);
    }

    private void Strafe()
    {
        Vector3 pathToTarget = target - transform.position;
        Vector3 dir = Vector3.Cross(pathToTarget, Vector3.up);
        GetPathToTarget(transform.position + (dir * rotateDirection));
    }

    private void LookAtTarget()
    {
        Quaternion angleToTarget = Quaternion.LookRotation(target - turret.position);
        turret.rotation = Quaternion.Lerp(turret.rotation, angleToTarget, 10f * Time.deltaTime);
    }

    private float CalculateDistance(Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        return distance;
    }

    private float CalculateAngle()
    {
        //Vector3 pathToTarget = agent.steeringTarget - rb.position;
        // steeringTarget -> typically the first corner in the path according to unity API


        //Vector3 t = (path.corners.Length > 0 ? path.corners[0] : target);
        Vector3 t = path.corners[1];
        //Debug.Log(t.ToString());
        Vector3 pathToTarget = t - rb.position;
        return Vector3.SignedAngle(transform.forward, pathToTarget, transform.up);
    }

    private void GetRandomNavPoint()
    {
        //gets a random point roaming distance away
        Vector3 randomDirection = new Vector3(RandomWithinRange(roamingDistance), 0, RandomWithinRange(roamingDistance));
        randomDirection = Vector3.ClampMagnitude(randomDirection, roamingDistance);
        Debug.DrawRay(transform.position, randomDirection, Color.green, 2f);
        randomDirection += transform.position;

        //finds the closest point on the navmesh to the random point
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamingDistance, 1);
        target = hit.position;
    }

    private float RandomWithinRange(float r)
    {
        return Random.Range(-r, r);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        foreach (Vector3 v in path.corners)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(v, 2f);
        }
    }
}
