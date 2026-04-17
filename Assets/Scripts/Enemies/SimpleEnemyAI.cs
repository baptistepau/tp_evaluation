using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { Patrol, Chase, Attack }
public class SimpleEnemyAI : MonoBehaviour
{
    [Header("animation")]
    public Animator animator;
    public SimpleNavigation navigation;

    [Header("détection")]
    public Transform player; // Référence au XR Origin
    public float viewAngle = 90f; // Largeur du cône
    public float viewDistance = 10f; // Portée
    public float timeToForgetPlayer = 5f; // Temps avant d'oublier le joueur

    [Header("attaque")]
    public float attackRange = 2f; // Distance d'attaque


    public EnemyState currentState;
    private NavMeshAgent agent;
    private float lastTimeSeePlayer;
    public bool isAttacking = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;
    }


    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                // Logique de patrouille
                if (CanSeePlayer())
                {
                    currentState = EnemyState.Chase;
                    navigation.enabled = false;
                }
                break;
            case EnemyState.Chase:
                // Logique de poursuite
                Chase();
                break;
            case EnemyState.Attack:
                // Logique d'attaque
                Attack();
                break;
        }
    }

    public bool CanSeePlayer()
    {
        //TODO a remplir
        return false;
    }

    public void Attack()
    {
        //TODO a remplir
    }

    public void Chase()
    {
        //TODO a remplir
    }
}
