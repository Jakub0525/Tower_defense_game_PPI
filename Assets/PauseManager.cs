using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the in-game pause system state and simulation pacing.
/// Monitors user input key toggles, manipulates global engine time scaling metrics, 
/// toggles the layout UI overlay overlays, and facilitates safe scene state termination.
/// </summary>
public class PauseManager : MonoBehaviour
{
    /// <summary>The UI panel canvas overlay displayed to the player during a paused game state.</summary>
    public GameObject pausePanel;

    /// <summary>Internal state flag tracker monitoring if the game timeline simulation is currently frozen.</summary>
    private bool isPaused = false;

    /// <summary>
    /// Standard Unity callback. Listens frame-by-frame for the designated pause input key (P), 
    /// dynamically switching simulation playback states based on the active tracking flags.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    /// <summary>
    /// Restores the global time scale factor back to native execution speed (1.0), 
    /// hides the canvas pause overlay layout, and clears the internal tracking flag.
    /// </summary>
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Freezes the global time scale factor down to a complete halt (0.0), halting update sequences 
    /// for physics calculations and translations, reveals the canvas pause overlay layout, and updates flags.
    /// </summary>
    void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Safely reverts the global time scale factor to default runtime speed to avoid freezing the system 
    /// configuration permanently, then commands the SceneManager to load the introduction screen index 0.
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}