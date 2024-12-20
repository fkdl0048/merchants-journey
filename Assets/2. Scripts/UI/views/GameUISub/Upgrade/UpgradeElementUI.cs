using UnityEngine;
using UnityEngine.UI;
using Scripts.Utils;
using Scripts.Data;
using Scripts.Manager;
using TMPro;
using System.Collections.Generic;
using System;

namespace Scripts.UI.GameUISub
{
    public class UpgradeElementUI : MonoBehaviour
    {
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject upgradeElementContainer;
        [SerializeField] private GameObject upgradeElementPrefab;

        [Header("Colors")]
        [SerializeField] private Color activeColor = Color.yellow;
        [SerializeField] private Color inactiveColor = Color.gray;

        private int currentValue;
        private int maxValue;
        private int upgradeCost;
        private StatType statType;
        private SaveManager saveManager;
        private bool isInitialized = false;
        private List<Image> upgradeElements = new List<Image>();
        private Action onGoldUpdated;

        private void Awake()
        {
            saveManager = SaveManager.Instance;
        }

        public void Initialize(string statName, string description, int initialValue, int maxValue, int upgradeCost, Action onGoldUpdated)
        {
            if (saveManager == null)
            {
                Debug.LogError("SaveManager is null in UpgradeElementUI!");
                return;
            }

            this.currentValue = initialValue;
            this.maxValue = maxValue;
            this.upgradeCost = upgradeCost;
            this.statType = (StatType)System.Enum.Parse(typeof(StatType), statName);
            this.isInitialized = true;
            this.onGoldUpdated = onGoldUpdated;

            if (upgradeButton != null)
            {
                upgradeButton.onClick.AddListener(OnUpgradeClicked);
            }
            else
            {
                Debug.LogError("Upgrade button is null in UpgradeElementUI!");
            }

            CreateUpgradeElements();
            UpdateUI();
        }

        private void CreateUpgradeElements()
        {
            if (upgradeElementContainer == null || upgradeElementPrefab == null)
            {
                Debug.LogError("Upgrade element container or prefab is null!");
                return;
            }

            // Clear existing elements
            foreach (Transform child in upgradeElementContainer.transform)
            {
                Destroy(child.gameObject);
            }
            upgradeElements.Clear();

            // Create new elements
            for (int i = 0; i < maxValue; i++)
            {
                GameObject element = Instantiate(upgradeElementPrefab, upgradeElementContainer.transform);
                Image elementImage = element.GetComponent<Image>();
                if (elementImage != null)
                {
                    upgradeElements.Add(elementImage);
                }
            }
        }

        private void UpdateUI()
        {
            if (!isInitialized)
            {
                return;
            }

            try
            {
                if (upgradeText != null)
                {
                    upgradeText.text = $"{statType}";
                }
                else
                {
                    Debug.LogError("Upgrade text is null in UpgradeElementUI!");
                }

                if (descriptionText != null)
                {
                    int currentCost = upgradeCost * (currentValue + 1);
                    descriptionText.text = $"Cost: {currentCost}";
                }
                else
                {
                    Debug.LogError("Description text is null in UpgradeElementUI!");
                }

                // Update upgrade elements colors
                for (int i = 0; i < upgradeElements.Count; i++)
                {
                    if (upgradeElements[i] != null)
                    {
                        upgradeElements[i].color = i < currentValue ? activeColor : inactiveColor;
                    }
                }

                if (saveManager != null)
                {
                    GameData gameData = saveManager.GetGameData();
                    bool canUpgrade = currentValue < maxValue && gameData.HasEnoughResources(upgradeCost * (currentValue + 1));
                    
                    if (upgradeButton != null)
                    {
                        upgradeButton.interactable = canUpgrade;
                    }
                }
                else
                {
                    Debug.LogError("SaveManager is null when updating UI!");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in UpdateUI: {e.Message}\nStack trace: {e.StackTrace}");
            }
        }

        private void OnUpgradeClicked()
        {
            if (!isInitialized || saveManager == null)
            {
                return;
            }

            try
            {
                GameData gameData = saveManager.GetGameData();
                int cost = upgradeCost * (currentValue + 1);
                
                if (currentValue < maxValue && gameData.HasEnoughResources(cost))
                {
                    gameData.ConsumeResources(cost);
                    saveManager.SaveGameData(gameData);
                    currentValue++;
                    UpdateUI();
                    onGoldUpdated?.Invoke();  // 골드 업데이트 콜백 호출
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in OnUpgradeClicked: {e.Message}\nStack trace: {e.StackTrace}");
            }
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
