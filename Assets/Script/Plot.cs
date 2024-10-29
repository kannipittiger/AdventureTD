using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;   

    private void Start(){
        startColor = sr.color;
    }

    private void OnMouseEnter(){
        sr.color = hoverColor;
    }

    private void OnMouseExit(){
        sr.color = startColor;
    }

    private void OnMouseDown(){
        if(tower != null) return;
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if(towerToBuild.cost > LevelManager.main.currency){
            Debug.Log("You can't afford this tower");
            return;
        }
        if (!BuildManager.main.CanPlaceSelectedTower())
        {
        Debug.Log("Max placement limit reached for this tower type!");
        return;
        }
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        BuildManager.main.RegisterTowerPlacement();
    }
}
