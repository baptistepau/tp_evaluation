using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace DefaultNamespace
{
    public class MyAgentIA : Agent
    {
        // --- 1. DECLARATION DES VARIABLES (Ce qui manquait !) ---
        [Header("Paramètres de l'Agent")]
        public Transform targetCube;    // Glisse le cube ici dans l'inspecteur
        public float spawnRadius = 4f;  // Zone d'apparition
        public float moveSpeed = 10f;   // Puissance du mouvement
        
        private Rigidbody rb;           // Référence au Rigidbody de la sphère

        // --- 2. INITIALISATION ---
        public override void Initialize()
        {
            // On récupère le Rigidbody une seule fois au début
            rb = GetComponent<Rigidbody>();
        }

        // --- 3. REINITIALISATION DE L'EPISODE ---
        public override void OnEpisodeBegin()
        {
            // Reset position sphère
            this.transform.localPosition = Vector3.zero;

            // Reset physique
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Placement aléatoire du cube
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            targetCube.localPosition = new Vector3(randomPoint.x, 0.5f, randomPoint.y);
        }

        // --- 4. OBSERVATIONS ---
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localPosition); // 3 valeurs
            sensor.AddObservation(targetCube.localPosition);    // 3 valeurs
            sensor.AddObservation(rb.linearVelocity);                 // 3 valeurs
            // Total = 9 (Pense à régler "Space Size" sur 9 dans l'inspecteur)
        }

        // --- 5. ACTIONS ET RECOMPENSES ---
        public override void OnActionReceived(ActionBuffers actions)
        {
            // Mouvement
            Vector3 controlSignal = Vector3.zero;
            controlSignal.x = actions.ContinuousActions[0];
            controlSignal.z = actions.ContinuousActions[1];
            rb.AddForce(controlSignal * moveSpeed);

            // Malus de temps
            AddReward(-0.001f);

            // Vérification de la distance (Victoire)
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, targetCube.localPosition);
            if (distanceToTarget < 1.42f)
            {
                SetReward(1.0f);
                EndEpisode();
            }

            // Vérification de chute (Défaite)
            if (this.transform.localPosition.y < 0)
            {
                EndEpisode();
            }
        }

        // --- 6. CONTROLE MANUEL ---
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
            continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
        }
    }
}