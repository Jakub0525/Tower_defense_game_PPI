using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents the central player core or base structure that must be protected.
/// Tracks base durability points, updates the tracking user interface, manages hit sound effects,
/// handles the game over state termination sequence, and facilitates scene reloading mechanics.
/// </summary>
public class PlayerBase : MonoBehaviour
{
    /// <summary>The current health points remaining for the main player base core.</summary>
    public int hp = 100;

    /// <summary>The AudioSource component triggered to play an impact audio file when the base takes damage.</summary>
    public AudioSource damageSound;

    /// <summary>The UI panel canvas overlay displayed to the player upon structural failure and defeat.</summary>
    public GameObject gameOverScreen;

    /// <summary>Reference to the TextMeshPro UI text element displaying the base's durability status.</summary>
    public TextMeshProUGUI hpText;

    /// <summary>
    /// Standard Unity callback. Guarantees the time scale factor is reset to normal operational speed (1.0) 
    /// upon entering the level and synchronizes the active health display metrics.
    /// </summary>
    void Start()
    {
        Time.timeScale = 1f;
        UpdateHPText();
    }

    /// <summary>
    /// Deducts incoming mathematical payloads from the core base health pool, refreshes user interfaces,
    /// triggers hit acoustic sound assets, and evaluates if health thresholds have fallen to initiate defeat loops.
    /// </summary>
    /// <param name="damage">The amount of raw damage inflicted by an invading enemy unit.</param>
    public void ReceiveDamage(int damage)
    {
        hp -= damage;
        UpdateHPText();

        if (damageSound != null) damageSound.Play();

        if (hp <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Formats and updates the baseline text metrics inside the designated TextMeshPro container overlay layout.
    /// </summary>
    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = "BASE HP: " + hp.ToString();
        }
    }

    /// <summary>
    /// Triggers the defeat routine sequence, activating the dedicated Game Over graphical canvas 
    /// layout and freezing the global engine simulation timescale to a complete halt (0.0).
    /// </summary>
    void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Commands the SceneManager runtime pipeline to synchronously reload the currently active scene map configuration.
    /// Used by retry interface buttons to clean the simulation grid sandbox.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}