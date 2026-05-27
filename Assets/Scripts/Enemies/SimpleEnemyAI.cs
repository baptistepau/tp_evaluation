using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { GoToCrystal, ChasePlayer, Attack }

public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    public Transform crystal;
    private NavMeshAgent agent;
    public Animator animator;

    [Header("Détection (Vision)")]
    public float viewAngle = 90f;
    public float viewDistance = 15f;
    public LayerMask obstacleMask;

    [Header("Combat")]
    public float attackRange = 2.5f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    public EnemyState currentState = EnemyState.GoToCrystal;
    private Transform currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Trouver les cibles automatiquement au spawn
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (crystal == null) crystal = GameObject.Find("crystal").transform; 
        
        currentTarget = crystal;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.GoToCrystal:
                currentTarget = crystal;
                agent.SetDestination(crystal.position);
                
                if (CanSeePlayer()) 
                {
                    currentState = EnemyState.ChasePlayer;
                }
                else if (Vector3.Distance(transform.position, crystal.position) <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                break;

            case EnemyState.ChasePlayer:
                currentTarget = player;
                agent.SetDestination(player.position);
                
                if (!CanSeePlayer())
                {
                    currentState = EnemyState.GoToCrystal;
                }
                else if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:
                agent.isStopped = true;
                
                Vector3 direction = (currentTarget.position - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);

                if (Time.time >= nextAttackTime)
                {
                    AttackTarget();
                    nextAttackTime = Time.time + 1f / attackRate;
                }

                if (Vector3.Distance(transform.position, currentTarget.position) > attackRange)
                {
                    agent.isStopped = false;
                    currentState = (currentTarget == player) ? EnemyState.ChasePlayer : EnemyState.GoToCrystal;
                }
                break;
        }
        
        if (animator != null)
        {
            animator.SetFloat("velocity", agent.velocity.magnitude);
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < viewDistance)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void AttackTarget()
    {
        if (animator != null) animator.SetTrigger("attack");

        if (currentTarget == crystal)
        {
            crystal_target ct = crystal.GetComponent<crystal_target>();
            if (ct != null) ct.TakeDamage(10);
        }
        else if (currentTarget == player)
        {
            Debug.Log("Paf ! L'ennemi frappe le joueur !");
        }
    }
}