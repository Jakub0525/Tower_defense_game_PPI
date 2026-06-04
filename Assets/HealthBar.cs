using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the visual presentation of a floating world-space health bar.
/// Handles graphical filling percentage computations and implements a billboarding effect 
/// to ensure the interface element continuously faces the active gameplay camera viewport.
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>The UI Image component set to 'Filled' mode representing the remaining durability status.</summary>
    public Image fillImage;

    /// <summary>Cached reference to the main camera transform required to compute orientation alignments.</summary>
    private Transform mainCamera;

    /// <summary>
    /// Standard Unity callback. Localizes and caches the active scene main camera reference on startup.
    /// </summary>
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    /// <summary>
    /// Standard Unity callback. Executes after all regular Update loops to enforce a billboard rotation matrix,
    /// keeping the overlay flat against the screen regardless of positional camera shifts.
    /// </summary>
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }

    /// <summary>
    /// Calculates the quotient ratio between current and maximum durability pools, 
    /// updating the raw UI image fill threshold accordingly.
    /// </summary>
    /// <param name="currentHP">The current remaining health points of the associated unit or structure.</param>
    /// <param name="maxHP">The total structural capacity threshold of the associated unit or structure.</param>
    public void UpdateBar(int currentHP, int maxHP)
    {
        fillImage.fillAmount = (float)currentHP / maxHP;
    }
}