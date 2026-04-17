using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public float timer = 60f;
    public UnityEvent<int> onScoreChanged;

    [Header("Interface")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;

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

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                Debug.Log("Game Over");
                Time.timeScale = 0;
            }
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + timer.ToString("F1");
        }
    }

    public void AddScore(int points)
    {
        score += points;
        onScoreChanged?.Invoke(score);
    }
}