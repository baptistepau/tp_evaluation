using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10; 

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("La balle a explosé sur : " + collision.gameObject.name);

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}