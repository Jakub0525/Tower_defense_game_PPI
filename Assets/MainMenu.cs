using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles user interactions on the introductory screen scene.
/// Manages transitions into the main active gameplay sandbox layout,
/// controls settings overlay panel toggles, and handles application termination events.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>Reference to the UI overlay canvas container holding audio and wave configurations.</summary>
    public GameObject settingsPanel;

    /// <summary>
    /// Standard Unity callback. Guarantees that the configuration sub-menu remains disabled upon initial startup loading sequences.
    /// </summary>
    void Start()
    {
        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Commands the scene management pipeline to synchronously dispose of the menu hierarchy and load the active game map at index 1.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Activates and overlays the options and configurations settings canvas interface on top of the background layout.
    /// </summary>
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Deactivates and hides the options configuration settings canvas overlay interface.
    /// </summary>
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Logs a debug signature to the terminal runtime and issues an application shutdown command block targeting standalone builds.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QUIT GAME!");
        Application.Quit();
    }
}