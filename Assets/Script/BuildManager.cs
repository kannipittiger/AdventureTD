using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int selectedTower = 0;
  

    // Dictionary to track the number of times each tower type has been placed
    private Dictionary<int, int> towerPlacementCount = new Dictionary<int, int>();

    private void Awake()
    {
        main = this;

        // Initialize placement counts to zero for each tower type
        for (int i = 0; i < towers.Length; i++)
        {
            towerPlacementCount[i] = 0;
            Debug.Log($"Initialized tower placement count for tower {i} ({towers[i].name}) to 0.");
        }
    }

    public Tower GetSelectedTower()
    {
        Debug.Log($"Selected Tower: {towers[selectedTower].name}");
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
        Debug.Log($"Tower selection changed to: {towers[selectedTower].name}");
    }

    // Check if the selected tower can still be placed
    public bool CanPlaceSelectedTower()
    {
        int currentPlacementCount = towerPlacementCount[selectedTower];
        int maxPlacement = towers[selectedTower].maxPlacement;
        
        Debug.Log($"Attempting to place {towers[selectedTower].name}. Current count: {currentPlacementCount}/{maxPlacement}");
        
        return currentPlacementCount < maxPlacement;
    }

    // Call this method when placing a tower successfully
    public void RegisterTowerPlacement()
{
    if (!CanPlaceSelectedTower())
    {

        // Trigger feedback or actions, such as disabling placement or showing a message
        Debug.Log($"Cannot place {towers[selectedTower].name}. Maximum placement limit reached!");

        // You can add additional code here if you want to disable placement UI or notify the player
        return;
    }

    // Increase the placement count if we haven't reached the max limit
    towerPlacementCount[selectedTower]++;
    Debug.Log($"Placed {towers[selectedTower].name}. New count: {towerPlacementCount[selectedTower]}");
}

}
