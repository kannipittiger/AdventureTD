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

    private Heroes currentHero; 
    private int currentUpgradeCost = 70;
    private int countUpgrade = 0;
    

    private void Start()
    {
        upgradePanel.SetActive(false);

    }

    private void Update()
    {
        if (countUpgrade > 2)
        {
            MaxUpgrade();
        }
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
        float currentDamage = currentHero.Damage;
        float currentRange = currentHero.targetingRange;
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
        countUpgradeText.text = $"Upgrade ({countUpgrade})";
    }

    public void ApplyUpgrade()
    {
        if (countUpgrade <= 2)
        {

            upgradeButtonText.text = $"Upgrade";
            if (LevelManager.main.currency >= Mathf.RoundToInt(currentUpgradeCost))
            {
                LevelManager.main.currency -= Mathf.RoundToInt(currentUpgradeCost);
                currentHero.UpgradeStats();
                countUpgrade++;
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
    }
}