using UnityEngine;
using TMPro; // N'oubliez pas d'ajouter cette ligne tout en haut !

public class Crystal_Target : MonoBehaviour
{
    [Header("Paramètres de Vie")]
    public int Health = 50;

    [Header("UI")]
    // Cette variable va stocker la référence à votre texte
    public TextMeshPro textMeshHealth; 

    void Start()
    {
        // On met à jour le texte dès le lancement du jeu
        UpdateHealthText();
    }

    // Créez une fonction pour mettre à jour le texte
    public void UpdateHealthText()
    {
        if (textMeshHealth != null)
        {
            textMeshHealth.text = Health.ToString();
        }
    }

    // Exemple de fonction quand le cristal prend des dégâts
    public void TakeDamage(int damage)
    {
        Health -= damage;
        UpdateHealthText(); // On met à jour l'affichage à chaque dégât
        
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Ce qu'il se passe quand le cristal est détruit
        Destroy(gameObject);
    }
}