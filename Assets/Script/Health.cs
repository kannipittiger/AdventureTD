using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health main;
    [Header("Attributes")]
    
    [SerializeField] private int currencyWorth = 50;
    public int currentHP;
    public int hitPoints = 30;

    private bool isDestroyed = false;

    private void Awake(){
        main = this;
        currentHP = hitPoints;
    }
    public void TakeDamage(int dmg){
        currentHP -= dmg;
        if(currentHP <= 0 && !isDestroyed){
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    public void SetHealth(int newHealth)
    {
        currentHP = newHealth;
        Debug.Log("Enemy health updated: " + currentHP);
    }
    public void SetCurrencyWorth(int worth)
    {
        currencyWorth = worth;
        Debug.Log("Enemy currency worth updated: " + currencyWorth);
    }
    public void ResetCurrencyWorth(){
        currencyWorth = 50;
    }
}
