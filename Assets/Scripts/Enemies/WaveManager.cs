using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject enemyPrefab; 
    public Transform[] spawnPoints; 

    [Header("Progression")]
    public int currentWave = 0;
    public int enemiesToSpawn = 2; 
    public int activeEnemies = 0;
    
    private bool waveInProgress = false;

    // Cette fonction est appelée automatiquement au lancement du jeu
    private void Start()
    {
        Debug.Log("Démarrage automatique de la première vague pour tester !");
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (waveInProgress) return;

        currentWave++;
        
        if (currentWave > 5)
        {
            Debug.Log("VICTOIRE ! Vous avez défendu le cristal !");
            return;
        }

        Debug.Log("Vague " + currentWave + " lancée !");
        waveInProgress = true;
        
        StartCoroutine(SpawnWaveGroup(enemiesToSpawn));
        StartCoroutine(IncreaseDifficultyEvery30Seconds());
    }

    IEnumerator SpawnWaveGroup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnSingleEnemy();
            yield return new WaitForSeconds(1f); 
        }
    }

    IEnumerator IncreaseDifficultyEvery30Seconds()
    {
        while (waveInProgress)
        {
            yield return new WaitForSeconds(30f);
            if (waveInProgress)
            {
                Debug.Log("30 secondes écoulées : Renforts ennemis !");
                enemiesToSpawn += 2; 
                StartCoroutine(SpawnWaveGroup(enemiesToSpawn));
            }
        }
    }

    void SpawnSingleEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, randomSpawn.position, randomSpawn.rotation);
        
        activeEnemies++;
    }

    public void OnEnemyDefeated()
    {
        activeEnemies--;
        
        if (activeEnemies <= 0 && waveInProgress)
        {
            EndWave();
        }
    }

    void EndWave()
    {
        waveInProgress = false;
        StopAllCoroutines(); 
        
        Debug.Log("Vague terminée ! Le cristal se régénère.");
    }
}