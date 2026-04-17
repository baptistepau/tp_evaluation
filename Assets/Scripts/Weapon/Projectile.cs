using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Projectile a touché " +  collision.gameObject.name);
        //TODO BONUS : ajouter des effets de particules quand on touche une surface.
    }
}
