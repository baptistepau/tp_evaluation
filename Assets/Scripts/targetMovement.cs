using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [Header("Réglages du mouvement")]
    public float speed = 3f;      // Vitesse de l'aller-retour
    public float distance = 6f;   // Distance totale parcourue (3m à gauche, 3m à droite)
    
    private Vector3 startPos;     // Pour se souvenir où était la cible au début

    void Start()
    {
        // On sauvegarde la position initiale de la cible
        startPos = transform.position;
    }

    void Update()
    {
        // Mathf.PingPong renvoie une valeur qui oscille entre 0 et 'distance'
        // Time.time * speed permet de faire avancer le compteur
        float offset = Mathf.PingPong(Time.time * speed, distance);

        // Calcul de la nouvelle position :
        // 1. On part de la position de départ (startPos)
        // 2. On ajoute le mouvement sur l'axe X (Vector3.right)
        // 3. On soustrait (distance / 2) pour que la cible aille aussi bien à gauche qu'à droite du point central
        transform.position = startPos + new Vector3(offset - (distance / 2), 0, 0);
    }
}