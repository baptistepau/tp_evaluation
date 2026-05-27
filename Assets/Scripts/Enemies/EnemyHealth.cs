using UnityEngine;
using UnityEngine.UI; // Indispensable pour utiliser la barre de vie (Slider) !

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    [Header("Interface")]
    public Slider healthBar; // Case pour glisser notre barre de vie

    void Start()
    {
        currentHealth = maxHealth;
        
        // On règle la barre de vie au maximum dès le départ
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // On baisse le niveau visuel de la barre
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnEnemyDefeated();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(1);
        }

        Destroy(gameObject);
    }
}