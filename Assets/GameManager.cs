using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int gold = 100;
    public TextMeshProUGUI goldText;

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

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

    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "GOLD: " + gold;
        }
    }
}