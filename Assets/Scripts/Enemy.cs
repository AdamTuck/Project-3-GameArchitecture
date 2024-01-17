using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint;

    private NavMeshAgent agent;


    // Adam's Nonsense
    private float roamTimer;
    [SerializeField] private float roamFrequency;
    [SerializeField] private float roamRadius;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = destinationPoint.transform.position;

        agent.SetDestination(RandomNavmeshLocation(roamRadius));
    }

    // Update is called once per frame
    void Update()
    {
        AgentRoam();
    }

    private void AgentRoam ()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= roamFrequency)
        {
            agent.SetDestination(RandomNavmeshLocation(roamRadius));
            roamTimer = 0;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.SetDestination(RandomNavmeshLocation(roamRadius));
                    roamTimer = 0;
                }
            }
        }
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
