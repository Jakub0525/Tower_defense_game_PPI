using UnityEngine;

/// <summary>
/// Centralized audio controller acting as a persistent manager to execute 
/// global user interface and environmental sound effects (SFX) without interruption.
/// Implements a basic Singleton pattern for quick cross-scene accessibility.
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>Static self-reference executing the Singleton pattern for global cross-script access.</summary>
    public static SoundManager instance;

    /// <summary>The primary AudioSource component responsible for playing one-shot acoustic sound effects.</summary>
    public AudioSource audioSource;

    /// <summary>Audio clip triggered upon successful structural placement or building upgrade execution.</summary>
    public AudioClip upgradeSound;

    /// <summary>Audio clip triggered when a building structure is demolished or sold by the player.</summary>
    public AudioClip deleteSound;

    /// <summary>Audio clip triggered by interactive user interface button pointer click elements.</summary>
    public AudioClip clickSound;

    /// <summary>
    /// Standard Unity callback. Configures the static instance reference layout immediately upon object awakening.
    /// </summary>
    void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Dispatches the upgrade/construction sound effect as a single non-blocking audio overlay payload.
    /// </summary>
    public void PlayUpgradeOrBuild()
    {
        audioSource.PlayOneShot(upgradeSound);
    }

    /// <summary>
    /// Dispatches the destruction/sell sound effect as a single non-blocking audio overlay payload.
    /// </summary>
    public void PlayDelete()
    {
        audioSource.PlayOneShot(deleteSound);
    }

    /// <summary>
    /// Dispatches the user interface element click sound effect as a single non-blocking audio overlay payload.
    /// </summary>
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}