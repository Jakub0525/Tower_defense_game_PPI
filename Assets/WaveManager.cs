using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject enemyPrefab;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public float timeBetweenWaves = 10f;
    private float countdown;

    private int waveIndex = 0;
    private bool allWavesSent = false;

    public TextMeshProUGUI waveCountdownText;
    public GameObject winScreen;

    void Start()
    {
        countdown = timeBetweenWaves;
        if (winScreen != null) winScreen.SetActive(false);
    }

    void Update()
    {
        if (allWavesSent)
        {
            CheckWinCondition();
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        if (waveCountdownText != null)
        {
            waveCountdownText.text = "Next wave in: " + Mathf.CeilToInt(countdown).ToString() + "s";
        }
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= waves.Length) yield break;

        Wave currentWave = waves[waveIndex];
        waveIndex++;

        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            yield return new WaitForSeconds(1f / currentWave.rate);
        }

        if (waveIndex >= waves.Length)
        {
            allWavesSent = true;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }

    void CheckWinCondition()
    {
        if (waveCountdownText != null) waveCountdownText.text = "All waves sent!";

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (activeEnemies.Length == 0 && allWavesSent)
        {
            if (winScreen != null && !winScreen.activeSelf)
            {
                winScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}