using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    public GameObject playerScript;
    NavMeshAgent agent;
    Vector3 DestPoint;
    public List<Transform> patrolPoints;
    public float waitTime = 2f;
    public float walkspeed = 2f;
    public float agrospeed = 6.5f;
    public float attackCooldown = 2.2f;
    Transform LastDestination;
    Transform CurrentDestination;
    Transform nextDestination;
    [SerializeField] float SightRange, AttackRange;
    [SerializeField] LayerMask GroundLayer, PlayerLayer;
    [SerializeField] float range;
    bool HitDestPoint;
    bool WalkPointSet;
    bool isWaiting = false;
    bool hasReactedToPlayer = false;
    bool canAttack = true;
    bool PlayerInSight, PlayerInAttackRange;
    BoxCollider AttackCollider;
    //Health
    public float maxHealth = 100;
    public float currentHealth; //take damage: currentHealth -= 20;
    [SerializeField] public HealthBar healthBar;//use healthBar.UpdateHealthBar(maxHealth, currentHealth); anytime the player takes damage
    public Rigidbody body;

    void Start()
    {
        if (healthBar == null)
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        Rigidbody rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerScript = GameObject.Find("Player");
        AttackCollider = GetComponentInChildren<BoxCollider>();
        SearchForDest();
        agent.speed = walkspeed;
    }
    void Chase()
    {

        agent.speed = agrospeed;
        agent.SetDestination(playerScript.transform.position);
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

    IEnumerator ReactToPlayer()
    {
        hasReactedToPlayer = true;
        agent.SetDestination(transform.position); // Stop moving
        yield return new WaitForSeconds(1.5f);     // Wait for 1.5 seconds
        hasReactedToPlayer = false;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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

        DestPoint = CurrentDestination.position;
        WalkPointSet = true;

       // agent.SetDestination(CurrentDestination.position);

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
        //agent.SetDestination(transform.position);
    }

    void EnableAttack()
    {
        AttackCollider.enabled = true;
    }
    void DesableAttack()
    {
        AttackCollider.enabled = false;
    }

    //public void OnCollision(Collider other)
    //{
    //    var Player = other.GetComponent<PlayerController>();
    //    if (Player != null)
    //        if (other.gameObject.CompareTag("Player"))
    //        {
    //            Debug.Log("Collided with player.");
    //            PlayerController playerScript = other.GetComponent<PlayerController>();
    //            if (playerScript == null) Debug.LogError("PlayerController not found");
    //            else if (playerScript != null)
    //            {
    //                // apply damage to player
    //                currentHealth -= 50;
    //                healthBar.UpdateHealthBar(maxHealth, currentHealth);

    //                StartCoroutine(AttackCooldown());
    //                Debug.Log("hit");
    //            }
    //        }
    //}

    /*public void OnTriggerEnter(Collider other)
    {
        if(!canAttack) return;
        if (other.CompareTag("Player"))
        {
            PlayerController playerScript = other.GetComponent<PlayerController> ();
            if(playerScript != null)
            {
                playerScript.TakeDamage(25); //call the method on PlayerController instead of editing health directly
                healthBar.UpdateHealthBar(maxHealth, currentHealth);
                StartCoroutine(AttackCooldown());
                Debug.Log("Enemy hit the Player!");
            }
        }
    }*/

    //void OnTriggerStay(Collider other)
    //{
    //    if (!canAttack) return;

    //    if (other.CompareTag("Player"))
    //    {
    //        float distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);

    //        if (distanceToPlayer <= AttackRange)
    //        {
    //            PlayerController playerController = other.GetComponent<PlayerController>();
    //            if (playerController != null)
    //            {
    //                playerController.TakeDamage(50); // Apply damage
    //                StartCoroutine(AttackCooldown());
    //                Debug.Log("Player damaged by proximity inside trigger.");
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);

        float distanceToPlayer = Vector3.Distance(transform.position, playerScript.transform.position);

        if (distanceToPlayer <= AttackRange && canAttack)
        {
            PlayerController playerController = playerScript.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(50);
                StartCoroutine(AttackCooldown());
                Debug.Log("Player damaged to proximity");
            }
        }

        if (!PlayerInSight && !PlayerInAttackRange)
        {
            Patrol();
        }
        else if (PlayerInSight && !PlayerInAttackRange)
        {
            if (!hasReactedToPlayer)
            {
                StartCoroutine(ReactToPlayer());
            }
            else
            {
                Chase();
            }
        }
        else if (PlayerInSight && PlayerInAttackRange)
        {
            Attack();
        }

        //if (!PlayerInSight && !PlayerInAttackRange) Patrol();
        //if (PlayerInSight && !PlayerInAttackRange) Chase();
        //if (PlayerInSight && PlayerInAttackRange) Attack();

        //if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 1f)
        {
            StartCoroutine(WaitAtPoint());
        }

    }
}
