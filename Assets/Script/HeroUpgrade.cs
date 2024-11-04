using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HeroUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI currentDamageText;
    [SerializeField] private TextMeshProUGUI currentRangeText;
    [SerializeField] private TextMeshProUGUI nextDamageText;
    [SerializeField] private TextMeshProUGUI nextRangeText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI countUpgradeText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Image heroPic;
    [SerializeField] private Image ArrowUp;
    [SerializeField] private Image ArrowDown;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton; // Reference to the sell button

    private Heroes currentHero;
    private BuildManager buildManager;

    // private int countUpgrade = 0;

    private void Start()
    {
        upgradePanel.SetActive(false);
        sellButton.onClick.AddListener(SellHero); // Add listener to the sell button
        buildManager = BuildManager.main; // อ้างอิงถึง BuildManager
    }


    public void Initialize(Heroes hero)
    {
        currentHero = hero;
        UpdateUpgradeUI();
        // Set the hero sprite in the UI image
        Sprite heroSprite = hero.GetHeroSprite();
        if (heroSprite != null)
        {
            heroPic.sprite = heroSprite;
        }
        else
        {
            Debug.LogError("Hero sprite is null.");
        }
    }

    public void ToggleUpgradePanel(bool show)
    {
        upgradePanel.SetActive(show);
    }

    public void CloseBTN()
    {
        upgradePanel.SetActive(false);
    }

    private void UpdateUpgradeUI()
    {
        if (currentHero.countUpgrade > 2)
        {
            MaxUpgrade();
        }
        else
        {
            float currentDamage = currentHero.CurrentDamage;
            float currentRange = currentHero.targetingRange;
            float currentUpgradeCost = currentHero.UpgradeCost;
            currentRange.ToString("0.00");
            float nextDamage = currentDamage * 1.4f;
            float nextRange = currentRange * 1.2f;
            float nextUpgrade = currentUpgradeCost * 1.2f;

            currentDamage = Mathf.RoundToInt(currentDamage);
            currentRange.ToString("0.00");
            nextDamage = Mathf.RoundToInt(nextDamage);
            nextRange.ToString("0.00");
            currentUpgradeCost = Mathf.RoundToInt(nextUpgrade);

            currentDamageText.text = $"DMG: {currentDamage}";
            currentRangeText.text = $"RNG: {currentRange:F2}";
            nextDamageText.text = $"{nextDamage}";
            nextRangeText.text = $"{nextRange:F2}";
            upgradeCostText.text = $"{currentUpgradeCost}$";
            countUpgradeText.text = $"Upgrade ({currentHero.countUpgrade})";
            upgradeButtonText.text = $"Upgrade";

            ArrowUp.gameObject.SetActive(true);
            ArrowDown.gameObject.SetActive(true);
            nextDamageText.gameObject.SetActive(true);
            nextRangeText.gameObject.SetActive(true);
            upgradeCostText.gameObject.SetActive(true);
        }

    }

    public void ApplyUpgrade()
    {

        if (currentHero.countUpgrade <= 2)
        {

            if (LevelManager.main.currency >= Mathf.RoundToInt(currentHero.UpgradeCost))
            {
                LevelManager.main.currency -= Mathf.RoundToInt(currentHero.UpgradeCost);
                currentHero.UpgradeStats();
                currentHero.countUpgrade++;
                UpdateUpgradeUI();
            }
        }
        else
        {
            MaxUpgrade();
        }
    }

    private void MaxUpgrade()
    {
        countUpgradeText.text = $"Upgrade (Max)";
        upgradeButtonText.text = $"Max";
        ArrowUp.gameObject.SetActive(false);
        ArrowDown.gameObject.SetActive(false);
        nextDamageText.gameObject.SetActive(false);
        nextRangeText.gameObject.SetActive(false);
        upgradeCostText.gameObject.SetActive(false);
        upgradePanel.SetActive(false);
    }

    // Method to sell the hero
    private void SellHero()
    {
        if (currentHero != null)
        {
            // Calculate 70% of the hero's original cost
            int sellAmount = Mathf.RoundToInt(currentHero.OriginalCost * 0.7f);

            // Add the sell amount to the player's currency
            LevelManager.main.currency += sellAmount;

            // Decrease the placement count in BuildManager
            BuildManager.main.towerPlacementCount[currentHero.TowerIndex]--;

            // Remove the hero from the scene
            Destroy(currentHero.gameObject);

            // Close the upgrade panel
            upgradePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("No hero to sell.");
        }
    }


}
