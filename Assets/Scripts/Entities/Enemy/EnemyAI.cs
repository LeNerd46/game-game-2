using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    #region Old Code
    /*public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
    public Transform player;
    public NavMeshAgent agent;
    public LastPlayerLocation lastPlayerLocation;

    public LayerMask groundMask;
    public LayerMask playerMask;

    public Vector3 walkPoint;
    public Vector3 personalLastSighting;

    
    [Header("Properties")]
    public float timeBetweenAttacks;
    public float walkPointRange;
    public float range;
    public float maxAngle;
    public float inFovTime;

    public float sightRange;
    public float attackRange;
    public float hearingRange;

    public bool stationary;

    [Tooltip("How long to wait until the enemy can walk to a new spot")]
    public int waitTime;
    public int damage;

    [Header("Random Stuff")]
    public int maxNumber;
    public int minNumber;

    [Header("Information")]
    public float playerDis;

    public bool playerInSightRange;
    public bool playerInAttackRange;
    public bool isInHearingRange;
    public bool playerFound;
    public bool foundPlayerDuringSearch;

    private bool waitedSeconds;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool inFOV;
    private bool canPatrol = true;

    [HideInInspector] public  bool alert;
    [HideInInspector] public bool personallyFound;

    public float currentTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (!playerFound && !inFOV && canPatrol && !stationary)
            Patrolling();
        if (playerFound && !playerInAttackRange)
            ChasePlayer();
        if (playerFound && playerInAttackRange)
            AttackPlayer();

        playerDis = CalculatePathLength(player.transform.position);
        inFOV = CheckFieldOfView(transform, player, maxAngle, sightRange);

        if (playerFound)
        {
            personalLastSighting = player.position;
            lastPlayerLocation.globalLastSeenPos = personalLastSighting;
        }

        if (lastPlayerLocation.playerSpotted && !personallyFound)
        {
            personalLastSighting = lastPlayerLocation.globalLastSeenPos;
            playerFound = true;
        }


        if (inFOV)
        {
            canPatrol = false;

            if (!playerFound)
            {
                // StartCoroutine(WaitForPlayer());

                if (currentTime <= inFovTime)
                    currentTime += Time.deltaTime;
            }
        }
        else
        {
            if (!playerFound)
                currentTime -= Time.deltaTime;

            if (currentTime <= 0)
                currentTime = 0;
        }
        
        if (currentTime >= inFovTime)
        {
            playerFound = true;
            lastPlayerLocation.playerSpotted = true;
            personallyFound = true;
            lastPlayerLocation.globalLastSeenPos = personalLastSighting;
        }
    }

    void Patrolling()
    {
        if (!walkPointSet)
            GetSearchPoint();
        else
            agent.SetDestination(walkPoint);

        Vector3 distance = transform.position - walkPoint;

        if (distance.magnitude < 1f && !waitedSeconds)
            StartCoroutine(WaitForWalkPoint());
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(personalLastSighting);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            Shoot();
        }
    }

    void GetSearchPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Shoot()
    {
        Vector3 direction = new Vector3(agent.transform.forward.x + Random.Range(-2, 2), agent.transform.forward.y, agent.transform.forward.z);
        Vector3 confirmedHitDirection = agent.transform.forward;

        if (Physics.Raycast(agent.transform.position, direction, out RaycastHit hit, range))
        {
            PlayerHealth health = hit.transform.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Missed the player");
            }
        }
        else
            Debug.Log("Missed the player");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 fovLineOne = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * sightRange;
        Vector3 fovLineTwo = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * sightRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLineOne);
        Gizmos.DrawRay(transform.position, fovLineTwo);

        if (!inFOV)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * sightRange);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * sightRange);
    }

    public static bool CheckFieldOfView(Transform checkingOject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[100];
        int count = Physics.OverlapSphereNonAlloc(checkingOject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingOject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingOject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingOject.position, target.position - checkingOject.position);

                        if (Physics.Raycast(ray, out RaycastHit hit, maxRadius))
                        {
                            if (hit.transform == target)
                                return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private float CalculatePathLength(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(targetPos, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPos;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    IEnumerator WaitForWalkPoint()
    {
        waitedSeconds = true;

        yield return new WaitForSeconds(waitTime);

        Debug.Log($"Waited {waitTime} seconds");

        walkPointSet = false;
        waitedSeconds = false;
    }

    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(inFovTime);

        if (inFOV)
            playerFound = true;
    }

    IEnumerator WaitToPatrol()
    {
        yield return new WaitForSeconds(2);

        if (!inFOV)
            canPatrol = true;
    }*/
    #endregion

    public LayerMask hidableLayer;
    public CheckLineOfSight losCheck;
    public NavMeshAgent agent;

    [Range(-1, 1)]
    [Tooltip("Lower is better for hiding spot")]
    public float hideSensitivity = 0f;

    [Range(1, 10)] public float minPlayerDistance = 5f;
    [Range(1, 5)] public float minObstacleHeight = 1.25f;
    [Range(0.01f, 1f)] public float updateFrequency = 0.25f;
    

    private Coroutine movementCoroutine;
    private Collider[] colliders = new Collider[10];

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        losCheck.OnGainSight += OnGainSight;
        losCheck.OnLoseSight += OnLoseSight;
    }

    private void OnGainSight(Transform target)
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(Hide(target));
    }

    private void OnLoseSight(Transform target)
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);
    }

    private IEnumerator Hide(Transform target)
    {
        WaitForSeconds wait = new WaitForSeconds(updateFrequency);

        while(true)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, losCheck.sphereCollider.radius, colliders, hidableLayer);
            int hitReduction = 0;
            
            for (int i = 0; i < hits; i++)
            {
                if(Vector3.Distance(colliders[i].transform.position, target.transform.position)  < minPlayerDistance || colliders[i].bounds.size.y < minObstacleHeight)
                {
                    colliders[i] = null;
                    hitReduction++;
                }
            }

            hits -= hitReduction;

            System.Array.Sort(colliders, ColliderArraySortCompare);

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                        Debug.Log($"Unable to find edge close to {hit.position}");

                    if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < hideSensitivity)
                    {
                        agent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        // The previous location wasn't facing away from the player, so it'll try again on the other side of the object
                        if(NavMesh.SamplePosition(colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                        {
                            if(!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                                Debug.LogError($"Unable to ifnd edeg close to {hit2.position} (second attempt)");

                            if(Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < hideSensitivity)
                            {
                                agent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
                else
                    Debug.LogError($"Unable to find NavMesh near object {colliders[i].name} at {colliders[i].transform.position}");
            }

            yield return wait;
        }
    }

    private int ColliderArraySortCompare(Collider a, Collider b)
    {
        if (a == null && b != null)
            return 1;
        else if (a != null && b == null)
            return -1;
        else if(a == null && b == null)
            return 0;
        else
            return Vector3.Distance(agent.transform.position, a.transform.position).CompareTo(Vector3.Distance(agent.transform.position, b.transform.position));
    }
}
