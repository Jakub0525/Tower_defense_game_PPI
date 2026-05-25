using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 50;

    public GameObject wallPrefab;
    public int wallCost = 10;

    public GameObject buildPanel;
    private Vector3 savedPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            CheckTile();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CloseMenu();
        }
    }

    void CheckTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                savedPosition = hit.collider.transform.position + new Vector3(0, 1f, 0);
                buildPanel.SetActive(true);
            }
            else
            {
                CloseMenu();
            }
        }
    }

    public void CloseMenu()
    {
        if (buildPanel != null) buildPanel.SetActive(false);
    }

    public void BuildTower()
    {
        if (GameManager.instance.SpendGold(towerCost))
        {
            Instantiate(towerPrefab, savedPosition, Quaternion.identity);
            CloseMenu();
        }
        else
        {
            Debug.Log("Not enough gold for a tower!");
        }
    }

    public void BuildWall()
    {
        if (GameManager.instance.SpendGold(wallCost))
        {
            Instantiate(wallPrefab, savedPosition, Quaternion.identity);
            CloseMenu();
        }
        else
        {
            Debug.Log("Not enough gold for a wall!");
        }
    }
}