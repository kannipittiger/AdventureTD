using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public static Health main;
    [Header("Attributes")]

    [SerializeField] private int currencyWorth = 50;
    public int currentHP;
    public int hitPoints = 50;

    private bool isDestroyed = false;

    private void Awake()
    {
        main = this;
        currentHP = hitPoints;
    }
    public void TakeDamage(int dmg)
    {
        if (isDestroyed) return; // Exit early if already destroyed

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            // Trigger events based on the active scene
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "SampleScene")
            {
                EnemySpawner.onEnemyDestroy.Invoke();
            }
            else if (currentSceneName == "DemonScene")
            {
                EnemySpawnerV2.onEnemyDestroy.Invoke();
            }

            // Increase player currency
            LevelManager.main.IncreaseCurrency(currencyWorth);

            // Mark as destroyed and destroy the object
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    public void SetHealth(int newHealth)
    {
        currentHP = newHealth;
        // Debug.Log("Enemy health updated: " + currentHP);
    }
    public void SetCurrencyWorth(int worth)
    {
        currencyWorth = worth;
        // Debug.Log("Enemy currency worth updated: " + currencyWorth);
    }
    public void ResetCurrencyWorth()
    {
        currencyWorth = 50;
    }
}
