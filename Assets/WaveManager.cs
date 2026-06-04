using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Orchestrates the enemy wave system, controlling spawn frequencies, paces inter-mission 
/// countdowns, enforces PlayerPrefs configuration wave caps, and handles level victory conditions.
/// </summary>
public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// Nested data structure defining individual wave layouts and configuration parameters.
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        /// <summary>The identifier name assigned to this specific wave sequence.</summary>
        public string waveName;

        /// <summary>The prefabricated GameObject asset representing the enemy entity type to be spawned.</summary>
        public GameObject enemyPrefab;

        /// <summary>The total number of enemy entities to be instantiated during this wave.</summary>
        public int count;

        /// <summary>The spawning frequency modifier expressed in units instantiated per second.</summary>
        public float rate;
    }

    /// <summary>The master database array holding all predefined custom wave layouts configured in the inspector.</summary>
    public Wave[] waves;

    /// <summary>The mandatory pause duration delay in seconds separating consecutive wave iterations.</summary>
    public float timeBetweenWaves = 10f;

    /// <summary>Internal timer counting down delta seconds remaining until the next wave deployment triggers.</summary>
    private float countdown;

    /// <summary>Internal index tracking the historical sequence position of the currently active wave.</summary>
    private int waveIndex = 0;

    /// <summary>Internal state flag verifying if all scheduled wave routines have completed their spawning cycles.</summary>
    private bool allWavesSent = false;

    /// <summary>Reference to the TextMeshPro UI container displaying the active next-wave countdown clock.</summary>
    public TextMeshProUGUI waveCountdownText;

    /// <summary>The visual UI canvas overlay pane displayed to the player upon successful match victory.</summary>
    public GameObject winScreen;

    /// <summary>The effective total of wave cycles designated for the current match run based on configuration files.</summary>
    private int targetWaves;

    /// <summary>
    /// Standard Unity callback. Synchronizes audio properties, extracts configuration wave caps 
    /// from PlayerPrefs databases, validates array limits, and initializes countdown clocks.
    /// </summary>
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1f);

        // Fetch user choice wave limits and clip parameters safely against hardcoded array thresholds
        targetWaves = PlayerPrefs.GetInt("MaxWaves", waves.Length);
        if (targetWaves > waves.Length) targetWaves = waves.Length;

        countdown = timeBetweenWaves;
        if (winScreen != null) winScreen.SetActive(false);
    }

    /// <summary>
    /// Standard Unity callback. Evaluates victory routines if wave assets are exhausted, 
    /// increments countdown clocks, and pushes formatted temporal string trackers to the user interface.
    /// </summary>
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

    /// <summary>
    /// Timed coroutine routing sequence that extracts wave definitions, increments state arrays, 
    /// loops enemy instantiations based on frequency spacing calculations, and handles final deployment flags.
    /// </summary>
    /// <returns>An IEnumerator yields execution cadence pause frames relative to the calculated wave pace.</returns>
    IEnumerator SpawnWave()
    {
        if (waveIndex >= targetWaves) yield break;

        Wave currentWave = waves[waveIndex];
        waveIndex++;

        // Deploy enemy iterations matching configured count quantities
        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            yield return new WaitForSeconds(1f / currentWave.rate);
        }

        if (waveIndex >= targetWaves)
        {
            allWavesSent = true;
        }
    }

    /// <summary>
    /// Instantiates the specified enemy archetype prefab at the position and rotation coordinates of this manager object.
    /// </summary>
    /// <param name="enemy">The enemy GameObject asset to instantiate into the gameplay scene.</param>
    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }

    /// <summary>
    /// Redraws UI labels and performs field sweeps looking for elements tagged as "Enemy". 
    /// If no active threats remain and all waves are spent, triggers the victory panel layout and halts time simulation.
    /// </summary>
    void CheckWinCondition()
    {
        if (waveCountdownText != null) waveCountdownText.text = "All waves sent!";

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Execute level victory routine loops when the battlefield context is entirely cleared of enemies
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