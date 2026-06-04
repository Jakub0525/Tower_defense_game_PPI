using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Manages player selection interactions with constructed buildings on the field.
/// Handles raycast object identification, manages the contextual upgrade/sell UI panel overlay, 
/// and dispatches transactions for structural level progression or demolition.
/// </summary>
public class SelectionManager : MonoBehaviour
{
    /// <summary>The currently targeted or selected building GameObject instance reference.</summary>
    private GameObject selectedBuilding;

    /// <summary>The UI layout panel canvas component exposing interactive upgrade and sell command buttons.</summary>
    public GameObject upgradePanel;

    /// <summary>The TextMeshPro UI element displaying technical building status, stats, and leveling costs.</summary>
    public TextMeshProUGUI infoText;

    /// <summary>
    /// Standard Unity callback. Monitors frame-by-frame mouse inputs to process structure selections (LMB)
    /// while blocking interaction passes through active UI elements, and clearing context panels (RMB).
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Abort selection if the mouse pointer is overlapping an interactive UI graphical element
            if (EventSystem.current.IsPointerOverGameObject()) return;

            SelectBuilding();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    /// <summary>
    /// Casts a ray from the camera lens array to the mouse pointer position. 
    /// If an entity with the "Building" tag is encountered, caches its instance identity and reveals contextual UI.
    /// </summary>
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

    /// <summary>
    /// Queries the selected building's data properties to update strings inside the info text block and reveals the panel.
    /// </summary>
    void ShowUpgradeUI()
    {
        Building b = selectedBuilding.GetComponent<Building>();
        upgradePanel.SetActive(true);
        infoText.text = "Level: " + b.level + "\nUpgrade: " + b.upgradeCost + "g";
    }

    /// <summary>
    /// Clears any stored structure references and deactivates the structural configuration canvas overlay.
    /// </summary>
    public void Deselect()
    {
        selectedBuilding = null;
        upgradePanel.SetActive(false);
    }

    /// <summary>
    /// UI Button Callback. Checks validation status, queries the financial system loop to deduct gold, 
    /// increments baseline building stats, triggers offensive stats scaling if a Tower component is bound, and redraws UI text.
    /// </summary>
    public void OnUpgradePressed()
    {
        if (selectedBuilding == null) return;

        Building b = selectedBuilding.GetComponent<Building>();

        if (GameManager.instance.SpendGold(b.upgradeCost))
        {
            b.UpgradeBuilding();
            if (SoundManager.instance != null) SoundManager.instance.PlayUpgradeOrBuild();

            // Check if the base building structure also contains a localized Tower offensive script layout
            Tower t = selectedBuilding.GetComponent<Tower>();
            if (t != null) t.UpgradeTower();

            ShowUpgradeUI();
        }
    }

    /// <summary>
    /// UI Button Callback. Dispatches gold refunds back to the economy management loops, 
    /// commands acoustic sound controllers to fire teardown effects, destroys the GameObject tracking reference, and clears UI views.
    /// </summary>
    public void OnSellPressed()
    {
        if (selectedBuilding == null) return;

        if (SoundManager.instance != null) SoundManager.instance.PlayDelete();
        Building b = selectedBuilding.GetComponent<Building>();
        GameManager.instance.AddGold(b.sellValue);

        Destroy(selectedBuilding);
        Deselect();
    }
}