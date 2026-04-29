using UnityEngine;

public class crystal_target : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DamageCrystal(damage);
        }
    }
}