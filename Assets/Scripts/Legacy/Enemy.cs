using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform enemyEye;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private float checkRadius = 0.8f;
    [SerializeField] private float enemyAggroRange, enemyAttackRange;

    int currentTarget = 0;
    private NavMeshAgent agent;

    public bool isPatrolling, isSearching;
    public bool isPlayerFound, isCloseToPlayer;

    public Transform player;

    // Roaming
    [Header("Searching")]
    [SerializeField] private float roamFrequency;
    [SerializeField] private float roamRadius;
    [SerializeField] private float searchTimeout;
    private float roamTimer, searchTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerFound)
        {
            if (isCloseToPlayer)
            {
                AttackPlayer();
            }
            else
            {
                FollowPlayer();
            }
        }
        else if (isSearching)
        {
            Searching();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol ()
    {
        if (agent.remainingDistance <= 0.2f)
        {
            currentTarget++;

            if (currentTarget >= patrolPoints.Length)
                currentTarget = 0;

            agent.destination = patrolPoints[currentTarget].position;
        }

        LookForPlayer();
    }

    private void LookForPlayer()
    {
        if (Physics.SphereCast(enemyEye.position, checkRadius, transform.forward, out RaycastHit hit, playerCheckDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("PLAYER SPOTTED");
                isPatrolling = false;
                isSearching = false;
                isPlayerFound = true;
                player = hit.transform;

                agent.destination = player.position;
            }
        }
    }

    void FollowPlayer ()
    {
        if (player)
        {
            if (Vector3.Distance(transform.position, player.position) > enemyAggroRange)
            {
                isPlayerFound = false;
                isSearching = true;
            }

            if (Vector3.Distance(transform.position, player.position) < enemyAttackRange)
            {
                isCloseToPlayer = true;
            }
            else
            {
                isCloseToPlayer = false;
            }

            agent.destination = player.position;
        }
        else
        {
            isCloseToPlayer = false;
            isSearching = true;
            isPlayerFound = false;
        }
    }

    void AttackPlayer()
    {
        Debug.Log("ATTACKING PLAYER");

        if (Vector3.Distance(transform.position, player.position) > enemyAttackRange)
        {
            isCloseToPlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyEye.position, checkRadius);
        Gizmos.DrawWireSphere(enemyEye.position + enemyEye.forward * playerCheckDistance, checkRadius);

        Gizmos.DrawLine(enemyEye.position, enemyEye.position + enemyEye.forward * playerCheckDistance);
    }

    private void Searching ()
    {
        roamTimer += Time.deltaTime;
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchTimeout)
        {
            isSearching = false;
            isPatrolling = true;
            roamTimer = 0;
            searchTimer = 0;
            return;
        }

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

        LookForPlayer();
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
