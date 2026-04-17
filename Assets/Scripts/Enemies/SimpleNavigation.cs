using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleNavigation : MonoBehaviour
{
    public enum NavigationBehavior
    {
        Reverse,    
        Loop,       
        Stop        
    }

    public List<Transform> waypoints = new List<Transform>();
    public float stoppingDistance = 0.1f; //Permet de s'arreter quand on est proche de la cible car le navMeshAgent peut parfois ne pas arriver a atteindre exxactement la cible.
    public NavigationBehavior endpointBehavior = NavigationBehavior.Loop;

    public int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;
    private NavMeshAgent agent;

    public Animator animator; 
    
    // Direction pour le mode Reverse (1 = avant, -1 = arrière)
    private int direction = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Count == 0)
        {
            Debug.LogWarning("SimpleNavigation: No waypoints assigned!");
            enabled = false;
        } else
        {
            MoveToNextWaypoint();
        }

    }

    void Update()
    {
        // Mise à jour de l'animation si un Animator est assigné
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        // Vérification si l'agent est arrivé à destination
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                MoveToNextWaypoint();
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        // Si le comportement est Stop et qu'on a atteint la fin, on ne fait plus rien
        if (endpointBehavior == NavigationBehavior.Stop && hasReachedEnd)
            return;

        // Définir la destination vers le waypoint actuel
        agent.destination = waypoints[currentWaypointIndex].position;

        // Calculer l'index du prochain waypoint selon le comportement choisi
        switch (endpointBehavior)
        {
            case NavigationBehavior.Loop:
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                break;

            case NavigationBehavior.Stop:
                if (currentWaypointIndex == waypoints.Count - 1)
                {
                    hasReachedEnd = true;
                }
                else
                {
                    currentWaypointIndex++;
                }
                break;

            case NavigationBehavior.Reverse:
                // Sécurité pour éviter les erreurs si on a 0 ou 1 waypoint
                if (waypoints.Count > 1)
                {
                    // Si on atteint la fin, on inverse la direction
                    if (direction == 1 && currentWaypointIndex == waypoints.Count - 1)
                    {
                        direction = -1;
                    }
                    // Si on atteint le début, on remet la direction en avant
                    else if (direction == -1 && currentWaypointIndex == 0)
                    {
                        direction = 1;
                    }
                    currentWaypointIndex += direction;
                }
                break;
        }
    }

}
