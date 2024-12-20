using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts.Utils;
using System.Collections.Generic;

namespace Scripts.UI.GameUISub
{
    public class UpgradeElementUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform costContainer;
        [SerializeField] private GameObject upgradeCostPrefab;
        
        private List<Image> costImages = new List<Image>();
        private int currentLevel = 1;
        private int maxLevel = 5;
        private int upgradeCost;
        private bool isVisible = true;

        public void Initialize(string statName, string description, int initialLevel, int maxLvl, int cost)
        {
            statNameText.text = statName;
            descriptionText.text = description;
            currentLevel = initialLevel;
            maxLevel = maxLvl;
            upgradeCost = cost;

            CreateCostElements();
            UpdateVisual();
        }

        private void CreateCostElements()
        {
            for (int i = 0; i < maxLevel; i++)
            {
                GameObject costElement = Instantiate(upgradeCostPrefab, costContainer);
                Image costImage = costElement.GetComponent<Image>();
                costImages.Add(costImage);
            }
            UpdateVisual();
        }

        public void OnUpgrade()
        {
            if (currentLevel < maxLevel)
            {
                currentLevel++;
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            for (int i = 0; i < costImages.Count; i++)
            {
                // 현재 레벨보다 낮거나 같은 경우 활성화된 상태로 표시
                costImages[i].color = i < currentLevel ? Color.yellow : Color.gray;
            }
        }

        public void SetVisible(bool visible)
        {
            isVisible = visible;
            gameObject.SetActive(visible);
        }

        public bool IsVisible() => isVisible;
    }
}
