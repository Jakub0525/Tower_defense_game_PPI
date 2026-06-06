using UnityEngine;

/// <summary>
/// Serves as the base framework for placeable structures (Towers and Walls) in the game.
/// Manages structural health, progression leveling, upgrade/sell economy, 
/// visual modifications, and hover-based UI visibility.
/// </summary>
public class Building : MonoBehaviour
{
    [Header("Health Settings")]
    /// <summary>The current remaining health points of the building.</summary>
    public int hp = 200;

    /// <summary>The maximum health capacity points of the building.</summary>
    public int maxHp = 200;

    [Header("Leveling Settings")]
    /// <summary>The current upgrade tier level of the structure.</summary>
    public int level = 1;

    /// <summary>The gold currency cost required to advance to the next upgrade tier.</summary>
    public int upgradeCost = 30;

    /// <summary>The gold currency value returned to the player upon selling the structure.</summary>
    public int sellValue = 20;

    [Header("Visuals")]
    /// <summary>The Renderer component used to dynamically alter the building's material color on upgrade.</summary>
    public Renderer modelRenderer;

    /// <summary>Reference to the UI HealthBar script to visually mirror health fluctuations.</summary>
    public HealthBar healthBar;

    /// <summary>The UI canvas GameObject containing info popups that toggle on mouse hover.</summary>
    public GameObject canvasObject;

    /// <summary>An array of custom colors used to visually indicate the current upgrade level tier.</summary>
    public Color[] upgradeColors;

    /// <summary>
    /// Standard Unity callback. Validates health parameter setup and syncs current health with maximum capacity.
    /// </summary>
    void Awake()
    {
        if (maxHp <= 0) maxHp = hp;

        hp = maxHp;
    }

    /// <summary>
    /// Standard Unity callback. Ensures that the status canvas overlay is disabled at the start of the game.
    /// </summary>
    void Start()
    {
        if (canvasObject != null) canvasObject.SetActive(false);
    }

    /// <summary>
    /// Increments the building level, applies exponential scaling multipliers to statistics 
    /// (HP and Costs), adjusts the visual material color, and refreshes the health bar UI.
    /// </summary>
    public void UpgradeBuilding()
    {
        level++;

        maxHp = Mathf.RoundToInt(maxHp * 1.25f);
        hp = maxHp;
        upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);
        sellValue += Mathf.RoundToInt(upgradeCost * 0.3f);

        if (modelRenderer != null && upgradeColors != null && upgradeColors.Length > 0)
        {
            int colorIndex = Mathf.Min(level - 2, upgradeColors.Length - 1);

            if (colorIndex >= 0)
            {
                modelRenderer.material.color = upgradeColors[colorIndex];
            }
        }

        if (healthBar != null) healthBar.UpdateBar(hp, maxHp);
        Debug.Log(gameObject.name + " upgraded to Level " + level + ". Max HP: " + maxHp);
    }

    /// <summary>
    /// Deducts a specified amount of damage from the structure's health. 
    /// Updates the health bar UI and handles self-destruction if health reaches zero or less.
    /// </summary>
    /// <param name="damage">The amount of damage inflicted on this building.</param>
    public void ReceiveDamage(int damage)
    {
        hp -= damage;

        if (healthBar != null) healthBar.UpdateBar(hp, maxHp);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unity mouse callback. Activates the overhead status canvas and updates the health bar when the pointer enters the collider.
    /// </summary>
    void OnMouseEnter()
    {
        if (canvasObject != null) canvasObject.SetActive(true);
        if (healthBar != null) healthBar.UpdateBar(hp, maxHp);
    }

    /// <summary>
    /// Unity mouse callback. Disables the overhead status canvas when the pointer exits the collider.
    /// </summary>
    void OnMouseExit()
    {
        if (canvasObject != null) canvasObject.SetActive(false);
    }
}