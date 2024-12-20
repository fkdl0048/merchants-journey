using UnityEngine;
using UnityEngine.UI;
using Scripts.Utils;
using TMPro;

namespace Scripts.UI.GameUISub
{
    public class UpgradeUnitPanelUI : MonoBehaviour
    {
        [Header("Select")]
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject selectBackground;
        [SerializeField] private TextMeshProUGUI unitNameText;
        
        [Header("Container")]
        [SerializeField] private Transform upgradeContainer;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject upgradeButtonPrefab;
        [SerializeField] private GameObject itemButtonPrefab;

        private UnitType unitType;
        
        private void Awake()
        {
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            // 초기 상태는 비활성화
            selectBackground.SetActive(false);
        }

        public void Initialize(UnitType type, string unitName)
        {
            unitType = type;
            if (unitNameText != null)
            {
                unitNameText.text = unitName;
            }
            CreateButtons();
        }
        
        public void SelectButton() => selectButton.onClick.Invoke();

        private void CreateButtons()
        {
            // 표도는 upgrade, item 버튼 둘 다 생성
            if (unitType == UnitType.Pyodu)
            {
                Instantiate(upgradeButtonPrefab, upgradeContainer);
                Instantiate(itemButtonPrefab, upgradeContainer);
            }
            // 표사는 upgrade 버튼만 생성
            else if (unitType == UnitType.Pyosa)
            {
                Instantiate(upgradeButtonPrefab, upgradeContainer);
            }
        }

        private void OnSelectButtonClicked()
        {
            // 같은 부모 아래의 모든 UpgradeUnitPanelUI 컴포넌트를 가져옴
            var allPanels = transform.parent.GetComponentsInChildren<UpgradeUnitPanelUI>();
            
            // 현재 선택된 패널의 인덱스 찾기
            int selectedIndex = System.Array.IndexOf(allPanels, this);
            
            if (selectedIndex >= 0)
            {
                // 선택된 패널을 맨 앞으로 이동
                transform.SetSiblingIndex(0);
                
                // 나머지 패널들의 선택 상태 해제
                foreach (var panel in allPanels)
                {
                    if (panel.selectBackground != null)
                    {
                        panel.selectBackground.SetActive(panel == this);
                    }
                }
            }
        }

        public void SetSelected(bool selected)
        {
            if (selectBackground != null)
            {
                selectBackground.SetActive(selected);
            }
        }
    }
}
