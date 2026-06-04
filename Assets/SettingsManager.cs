using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages configuration settings interfaces and user preference data persistence.
/// Synchronizes interactive UI slider components with global engine properties 
/// (such as AudioListener gain thresholds) and saves configurations via PlayerPrefs.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    /// <summary>The interactive UI slider component tracking master audio volume inputs.</summary>
    public Slider volumeSlider;

    /// <summary>The interactive UI slider component tracking target gameplay wave caps.</summary>
    public Slider waveSlider;

    /// <summary>The TextMeshPro UI layout element used to render volume metrics converted into a percentage string.</summary>
    public TextMeshProUGUI volumeText;

    /// <summary>The TextMeshPro UI layout element used to display the active numerical wave configuration cap.</summary>
    public TextMeshProUGUI waveText;

    /// <summary>
    /// Standard Unity callback. Pulls cached system configurations from local PlayerPrefs storage registers 
    /// (applying safe fallbacks if empty), configures baseline slider thresholds, and triggers update routines.
    /// </summary>
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("GlobalVolume", 1f);
        waveSlider.value = PlayerPrefs.GetInt("MaxWaves", 5);

        UpdateVolume();
        UpdateWaves();
    }

    /// <summary>
    /// UI Slider Callback. Queries the volume slider asset tracker, adjusts global audio listener gain factors, 
    /// saves values into the engine database profile, and formats screen overlay text percentages.
    /// </summary>
    public void UpdateVolume()
    {
        AudioListener.volume = volumeSlider.value;

        PlayerPrefs.SetFloat("GlobalVolume", volumeSlider.value);

        volumeText.text = "" + Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }

    /// <summary>
    /// UI Slider Callback. Queries the wave counter slider position matrix, updates internal parameter limits 
    /// inside player preference data lines, and synchronizes the target screen layout description text.
    /// </summary>
    public void UpdateWaves()
    {
        int currentWaves = (int)waveSlider.value;
        PlayerPrefs.SetInt("MaxWaves", currentWaves);
        waveText.text = "" + currentWaves;
    }
}