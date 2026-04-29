using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Statistiques")]
    public int score;
    public float timer = 60f;
    public int crystalHealth = 100; // On centralise la santé ici

    [Header("Joueur & Respawn")]
    public GameObject player; // Glisser le Player ici
    public Transform respawnPoint; // Glisser un objet vide placé au centre du labyrinthe

    public UnityEvent<int> onScoreChanged;

    [Header("Interface")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text crystalHealthText; // Pour afficher la vie du cristal sur le Canvas World Space

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateUI(); // Met à jour le texte

            if (timer <= 0)
            {
                timer = 0;
                GameOver(); 
            }
        }
    }

    // Fonction pour mettre à jour l'interface proprement
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (timerText != null) timerText.text = "Time: " + timer.ToString("F1");
        if (crystalHealthText != null) crystalHealthText.text = "" + crystalHealth;
    }

    // --- NOUVELLES FONCTIONS ---

    public void AddScore(int points)
    {
        score += points;
        onScoreChanged?.Invoke(score);
        UpdateUI();
    }

    // Fonction à appeler quand le cristal prend un coup
    public void DamageCrystal(int damage)
    {
        crystalHealth -= damage;
        UpdateUI();

        if (crystalHealth <= 0)
        {
            crystalHealth = 0;
            GameOver(); // Le jeu s'arrête si le Cristal est détruit
        }
    }

    // Fonction à appeler quand la vie du joueur tombe à 0
    public void PlayerDied()
    {
        Debug.Log("Le joueur est mort ! Respawn...");
        
        // Pénalité de score
        score -= 10; 
        if (score < 0) score = 0; // Pour éviter un score négatif
        UpdateUI();

        // Respawn au centre
        if (player != null && respawnPoint != null)
        {
            // Astuce : Si ton joueur utilise un CharacterController, il faut le désactiver juste avant de le téléporter, puis le réactiver
            player.SetActive(false);
            player.transform.position = respawnPoint.position;
            player.SetActive(true);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over !");
        Time.timeScale = 0; // Met le jeu en pause
    }
}