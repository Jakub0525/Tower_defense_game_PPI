using UnityEngine;

/// <summary>
/// Coordinates persistent background music playback across different scenes.
/// Implements a strict Singleton architectural pattern combined with Unity's 
/// DontDestroyOnLoad mechanism to ensure uninterrupted audio execution.
/// </summary>
public class MusicManager : MonoBehaviour
{
    /// <summary>Static self-reference caching the active global instance to prevent duplicate manager runtimes.</summary>
    private static MusicManager instance;

    /// <summary>
    /// Standard Unity callback. Evaluates if a prior manager instance exists in the current environment context. 
    /// Destroys redundant duplicate GameObjects and marks the valid instance as persistent across scene loads.
    /// </summary>
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}