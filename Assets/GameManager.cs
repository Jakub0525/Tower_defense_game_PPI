using UnityEngine;
using TMPro;

/// <summary>
/// Core gameplay director supervising the strategic resource economy loop.
/// Manages player gold assets, evaluates financial transaction validations, 
/// updates economic UI elements, and implements a basic Singleton runtime pattern.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>The current amount of gold currency accumulated by the player.</summary>
    public int gold = 100;

    /// <summary>Reference to the TextMeshPro UI component used to render the gold counter onto the screen.</summary>
    public TextMeshProUGUI goldText;

    /// <summary>Static self-reference executing the Singleton pattern for global cross-script access.</summary>
    public static GameManager instance;

    /// <summary>
    /// Standard Unity callback. Configures the static reference instance layout immediately upon loading the scene.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Standard Unity callback. Synchronizes the graphical user interface indicators with the baseline gold properties on start.
    /// </summary>
    void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Increments the player's gold reserve by a specified numerical payload and triggers a graphical interface redraw.
    /// </summary>
    /// <param name="amount">The quantity of gold currency earned (e.g., from an enemy kill or wave survival bonus).</param>
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    /// <summary>
    /// Evaluates financial criteria to verify if the current gold reserves can satisfy a specified transaction cost. 
    /// If valid, processes the gold deduction and redraws the user interface.
    /// </summary>
    /// <param name="cost">The resource gold price requested for building a structure or executing an upgrade loop.</param>
    /// <returns>True if the transaction was approved and processed successfully; false if funds were insufficient.</returns>
    public bool SpendGold(int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Formats and pushes the active currency count to the designated TextMeshPro container overlay.
    /// </summary>
    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "GOLD: " + gold;
        }
    }
}