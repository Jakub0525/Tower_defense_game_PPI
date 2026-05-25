using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Health Settings")]
    public int hp = 50;
    public int maxHp = 50;

    [Header("Leveling Settings")]
    public int level = 1;
    public int upgradeCost = 30;
    public int sellValue = 20;

    [Header("Visuals")]
    public MeshRenderer modelRenderer;
    public HealthBar healthBar;
    public GameObject canvasObject;
    public Color[] upgradeColors;

    void Awake()
    {
        if (maxHp <= 0) maxHp = hp;

        hp = maxHp;
    }

    void Start()
    {
        if (canvasObject != null) canvasObject.SetActive(false);
    }

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

    public void ReceiveDamage(int damage)
    {
        hp -= damage;

        if (healthBar != null) healthBar.UpdateBar(hp, maxHp);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        if (canvasObject != null) canvasObject.SetActive(true);
        if (healthBar != null) healthBar.UpdateBar(hp, maxHp);
    }

    void OnMouseExit()
    {
        if (canvasObject != null) canvasObject.SetActive(false);
    }
}