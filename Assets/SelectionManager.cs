using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    private GameObject selectedBuilding;
    public GameObject upgradePanel;
    public TextMeshProUGUI infoText;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            SelectBuilding();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    void SelectBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Building"))
            {
                selectedBuilding = hit.collider.gameObject;
                ShowUpgradeUI();
            }
            else
            {
                Deselect();
            }
        }
        else
        {
            Deselect();
        }
    }

    void ShowUpgradeUI()
    {
        Building b = selectedBuilding.GetComponent<Building>();
        upgradePanel.SetActive(true);
        infoText.text = "Level: " + b.level + "\nUpgrade: " + b.upgradeCost + "g";
    }

    public void Deselect()
    {
        selectedBuilding = null;
        upgradePanel.SetActive(false);
    }

    public void OnUpgradePressed()
    {
        if (selectedBuilding == null) return;

        Building b = selectedBuilding.GetComponent<Building>();

        if (GameManager.instance.SpendGold(b.upgradeCost))
        {
            b.UpgradeBuilding();

            Tower t = selectedBuilding.GetComponent<Tower>();
            if (t != null) t.UpgradeTower();

            ShowUpgradeUI();
        }
    }

    public void OnSellPressed()
    {
        if (selectedBuilding == null) return;

        Building b = selectedBuilding.GetComponent<Building>();
        GameManager.instance.AddGold(b.sellValue);

        Destroy(selectedBuilding);
        Deselect();
    }
}