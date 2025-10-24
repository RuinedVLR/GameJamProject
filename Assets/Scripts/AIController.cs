using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject Player;
    NavMeshAgent agent;
    Vector3 DestPoint;
    [SerializeField] float SightRange, AttackRange;
    [SerializeField] LayerMask GroundLayer, PlayerLayer;
    bool HitDestPoint;
    bool WalkPointSet;
    bool PlayerInSight, PlayerInAttackRange;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
    }
    void Chase()
    {
        agent.SetDestination(Player.transform.position);
    }
    void Patrol()
    {
        if (!WalkPointSet) ;
        if(WalkPointSet) agent.SetDestination(DestPoint);
        if(Vector3.Distance(transform.position, DestPoint)< 10) WalkPointSet = false;
    }

    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, SightRange);
    }
}
