using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject Player;
    NavMeshAgent agent;
    Vector3 DestPoint;
    public List<Transform> patrolPoints;
    public float waitTime = 2f;
    public float walkspeed = 2f;
    public float agrospeed = 6.5f;
    Transform LastDestination;
    Transform CurrentDestination;
    Transform nextDestination;
    [SerializeField] float SightRange, AttackRange;
    [SerializeField] LayerMask GroundLayer, PlayerLayer;
    [SerializeField] float range;
    bool HitDestPoint;
    bool WalkPointSet;
    bool isWaiting = false;
    bool PlayerInSight, PlayerInAttackRange;
    BoxCollider AttackCollider;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        AttackCollider = GetComponentInChildren<BoxCollider>();
        SearchForDest();
        agent.speed = walkspeed;
    }
    void Chase()
    {

        agent.speed = agrospeed;
        agent.SetDestination(Player.transform.position);
    }
    void Patrol()
    {
        agent.speed = walkspeed;
        if (!WalkPointSet) SearchForDest();
        if (WalkPointSet) agent.SetDestination(DestPoint);
        if (Vector3.Distance(transform.position, DestPoint) < 10) WalkPointSet = false;
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        SearchForDest();
        isWaiting = false;
    }

    void SearchForDest()
    {
        if (patrolPoints.Count == 0) return;
        
        do
        {
            nextDestination = patrolPoints[Random.Range(0, patrolPoints.Count)];
        } while (nextDestination == LastDestination && patrolPoints.Count > 1);
        CurrentDestination = nextDestination;
        LastDestination = CurrentDestination;
        agent.SetDestination(CurrentDestination.position);
        //float x = Random.Range(-range, range);
        //float z = Random.Range(-range, range);
        //DestPoint = new Vector3(transform.position.x + x, transform.position.y,transform.position.z + z);
        //if (Physics.Raycast(DestPoint, Vector3.down, GroundLayer))
        //{
        //    WalkPointSet = true;
        //}
    }
    void Attack()
    {
        agent.SetDestination(transform.position);
    }
    void EnableAttack()
    {
        AttackCollider.enabled = true;
    }
    void DesableAttack()
    {
        AttackCollider.enabled = false;
    }
    public void OnCollision(Collider other)
    {
        var Player = other.GetComponent<PlayerController>();
        if (Player != null)
            if (other.gameObject.CompareTag("Player"))
            {

                print("hit");
                Debug.Log("Collided with player.");

                PlayerController playerScript = other.GetComponent<PlayerController>();
                if (playerScript == null) Debug.LogError("PlayerController not found");

                else if (playerScript != null)
                {
                    // apply damage to player
                    playerScript.currentHealth -= 50;
                    print("hit");
                }
            }
    }

    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);
        if (!PlayerInSight && !PlayerInAttackRange) Patrol();
        if (PlayerInSight && !PlayerInAttackRange) Chase();
        if (PlayerInSight && PlayerInAttackRange) Attack();

        //if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAtPoint());
        }

    }
}
