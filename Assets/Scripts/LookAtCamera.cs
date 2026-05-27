using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // Vérifie qu'il y a bien une caméra dans la scène
        if (Camera.main != null)
        {
            // Fait pivoter l'objet pour qu'il regarde la caméra
            transform.LookAt(Camera.main.transform);
            
            // Le LookAt a tendance à afficher l'UI à l'envers (effet miroir), on le retourne de 180 degrés
            transform.Rotate(0, 180, 0);
        }
    }
}