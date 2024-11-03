using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] public Tower[] towers;

    private int selectedTower = -1;  // Initial value set to -1 to indicate no selection
    private bool hasSelectedTower = false; // New flag to check selection

    public Dictionary<int, int> towerPlacementCount = new Dictionary<int, int>();

    private void Awake()
    {
        main = this;

        for (int i = 0; i < towers.Length; i++)
        {
            towerPlacementCount[i] = 0;
            Debug.Log($"Initialized tower placement count for tower {i} ({towers[i].name}) to 0.");
        }
    }

    public Tower GetSelectedTower()
    {
        if (!hasSelectedTower)
        {
            Debug.Log("No tower selected!");
            return null;
        }

        Debug.Log($"Selected Tower: {towers[selectedTower].name}");
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
        hasSelectedTower = true; // Mark that a tower is selected
        Debug.Log($"Tower selection changed to: {towers[selectedTower].name}");
    }

    public bool CanPlaceSelectedTower()
    {
        if (!hasSelectedTower) return false; // Check if a tower is selected

        int currentPlacementCount = towerPlacementCount[selectedTower];
        int maxPlacement = towers[selectedTower].maxPlacement;

        Debug.Log($"Attempting to place {towers[selectedTower].name}. Current count: {currentPlacementCount}/{maxPlacement}");

        return currentPlacementCount < maxPlacement;
    }

    public void RegisterTowerPlacement()
    {
        if (!CanPlaceSelectedTower()) return;

        towerPlacementCount[selectedTower]++;
        hasSelectedTower = false; // Reset selection after placing the tower
        Debug.Log($"Placed {towers[selectedTower].name}. New count: {towerPlacementCount[selectedTower]}");
    }
}
