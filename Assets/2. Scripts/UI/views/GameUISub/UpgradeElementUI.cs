using UnityEngine;
using UnityEngine.UI;
using Scripts.Utils;
using Scripts.Data;
using TMPro;

public class UpgradeElementUI : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private GameObject upgradeCostContainer;
    [SerializeField] private GameObject upgradeCostPrefab;

    private int currentValue;
    private int maxValue;
    private int upgradeCost;
    private string description;
    private StatType statType;

    public void Initialize(string statName, string description, int initialValue, int maxValue, int upgradeCost)
    {
        this.currentValue = initialValue;
        this.maxValue = maxValue;
        this.upgradeCost = upgradeCost;
        this.description = description;
        this.statType = (StatType)System.Enum.Parse(typeof(StatType), statName);

        UpdateUI();
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }
    }

    private void UpdateUI()
    {
        if (upgradeText != null)
        {
            upgradeText.text = $"{statType}: {currentValue}/{maxValue}\n{description}";
        }

        bool canUpgrade = currentValue < maxValue;
        if (upgradeButton != null)
            upgradeButton.interactable = canUpgrade;

        // 비용 표시 업데이트
        foreach (Transform child in upgradeCostContainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (canUpgrade)
        {
            int remainingUpgrades = maxValue - currentValue;
            for (int i = 0; i < remainingUpgrades; i++)
            {
                var costObj = Instantiate(upgradeCostPrefab, upgradeCostContainer.transform);
                var costText = costObj.GetComponentInChildren<Text>();
                if (costText != null)
                {
                    costText.text = (upgradeCost * (i + 1)).ToString();
                }
            }
        }
    }

    private void OnUpgradeClicked()
    {
        if (currentValue < maxValue)
        {
            currentValue++;
            UpdateUI();
        }
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
