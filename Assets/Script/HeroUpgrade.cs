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
    [SerializeField] private Image heroPic;
    [SerializeField] private Button upgradeButton;

    private Heroes hero;
    private int currentUpgradeCost = 70;

    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void Initialize(Heroes hero)
    {
        this.hero = hero;
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
        float currentDamage = hero.damage;
        float currentRange = hero.targetingRange;
        currentRange.ToString("0.00");
        float nextDamage = currentDamage * 1.2f;  // Example increase, adjust as needed
        float nextRange = currentRange * 1.1f;    // Example increase, adjust as needed
        float nextUpgradeCost = currentUpgradeCost * 1.2f;

        currentDamage = Mathf.RoundToInt(currentDamage);
        currentRange.ToString("0.00");
        nextDamage = Mathf.RoundToInt(nextDamage);
        nextRange.ToString("0.00");
        currentUpgradeCost = Mathf.RoundToInt(nextUpgradeCost);

        currentDamageText.text = $"DMG: {currentDamage}";
        currentRangeText.text = $"RNG: {currentRange}";
        nextDamageText.text = $"{nextDamage}";
        nextRangeText.text = $"{nextRange}";
        upgradeCostText.text = $"{currentUpgradeCost}$";
    }

    public void ApplyUpgrade()
    {

        if (LevelManager.main.currency >= Mathf.RoundToInt(currentUpgradeCost))
        {
            LevelManager.main.currency -= Mathf.RoundToInt(currentUpgradeCost);
            hero.UpgradeStats();
            UpdateUpgradeUI();
        }
    }
}