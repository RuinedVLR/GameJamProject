using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject Player;
    NavMeshAgent agent;
    Vector3 DestPoint;
    [SerializeField] float SightRange, AttackRange;
    [SerializeField] LayerMask GroundLayer, PlayerLayer;
    [SerializeField] float range;
    bool HitDestPoint;
    bool WalkPointSet;
    bool PlayerInSight, PlayerInAttackRange;
    BoxCollider AttackCollider;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        AttackCollider = GetComponentInChildren<BoxCollider>();
    }
    void Chase()
    {
        agent.SetDestination(Player.transform.position);
    }
    void Patrol()
    {
        if (!WalkPointSet) SearchForDest();
        if(WalkPointSet) agent.SetDestination(DestPoint);
        if(Vector3.Distance(transform.position, DestPoint)< 10) WalkPointSet = false;
    }
    void SearchForDest()
    {
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        DestPoint = new Vector3(transform.position.x + x, transform.position.y,transform.position.z + z);
        if (Physics.Raycast(DestPoint, Vector3.down, GroundLayer))
        {
            WalkPointSet = true;
        }
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
    public void OnTriggerEnter(Collider other)
    {
        var Player = other.GetComponent<CharacterController>();
        if(Player != null)
        {
            print("hit");
        }
    }

    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange, PlayerLayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerLayer);
        if(!PlayerInSight && !PlayerInAttackRange)Patrol();
        if(PlayerInSight && !PlayerInAttackRange)Chase();
        if(PlayerInSight && PlayerInAttackRange)Attack();
    }
}
