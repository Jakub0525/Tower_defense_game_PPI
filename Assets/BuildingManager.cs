using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the placement and construction of structures on the game grid.
/// Handles player mouse input, screen-to-world raycasting to detect valid building tiles,
/// transaction processing via the GameManager, and structural instantiation.
/// </summary>
public class BuildingManager : MonoBehaviour
{
    /// <summary>The prefabricated GameObject asset for the offensive tower structure.</summary>
    public GameObject towerPrefab;

    /// <summary>The gold currency cost required to construct a single tower unit.</summary>
    public int towerCost = 50;

    /// <summary>The prefabricated GameObject asset for the defensive wall barrier structure.</summary>
    public GameObject wallPrefab;

    /// <summary>The gold currency cost required to construct a single wall unit.</summary>
    public int wallCost = 10;

    /// <summary>The UI panel overlay displaying construction structure options to the player.</summary>
    public GameObject buildPanel;

    /// <summary>The stored world space coordinate vector designated for structure instantiation.</summary>
    private Vector3 savedPosition;

    /// <summary>
    /// Standard Unity callback. Monitors player clicks per frame to handle construction tile selection (LMB)
    /// while avoiding UI click-throughs, and menu cancellation inputs (RMB).
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Abort raycast detection if the player clicks on a UI graphical element
            if (EventSystem.current.IsPointerOverGameObject()) return;

            CheckTile();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CloseMenu();
        }
    }

    /// <summary>
    /// Casts a ray from the main camera viewport through the mouse pointer position. 
    /// If an environment object with the "Tile" tag is hit, calculates placement coordinates and opens the build overlay menu.
    /// </summary>
    void CheckTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                // Offsets the instantiation position slightly upward to snap the building flush onto the tile surface
                savedPosition = hit.collider.transform.position + new Vector3(0, 1f, 0);
                buildPanel.SetActive(true);
            }
            else
            {
                CloseMenu();
            }
        }
    }

    /// <summary>
    /// Deactivates and hides the building selection UI panel element.
    /// </summary>
    public void CloseMenu()
    {
        if (buildPanel != null) buildPanel.SetActive(false);
    }

    /// <summary>
    /// Queries the GameManager economy to deduce gold currency for a tower unit. 
    /// If successful, triggers the placement audio effect, instantiates the tower prefab, and closes the menu overlay.
    /// </summary>
    public void BuildTower()
    {
        if (GameManager.instance.SpendGold(towerCost))
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayUpgradeOrBuild();
            Instantiate(towerPrefab, savedPosition, Quaternion.identity);
            CloseMenu();
        }
        else
        {
            Debug.Log("Not enough gold for a tower!");
        }
    }

    /// <summary>
    /// Queries the GameManager economy to deduce gold currency for a wall unit. 
    /// If successful, triggers the placement audio effect, instantiates the wall prefab, and closes the menu overlay.
    /// </summary>
    public void BuildWall()
    {
        if (GameManager.instance.SpendGold(wallCost))
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayUpgradeOrBuild();
            Instantiate(wallPrefab, savedPosition, Quaternion.identity);
            CloseMenu();
        }
        else
        {
            Debug.Log("Not enough gold for a wall!");
        }
    }
}