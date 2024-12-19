using UnityEngine;
using UnityEngine.UI;
using Scripts.Utils;

namespace Scripts.UI.GameUISub
{
    public class UpgradeUnitPanelUI : MonoBehaviour
    {
        [Header("Select")]
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject selectBackground;
        
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

        public void Initialize(UnitType type)
        {
            unitType = type;
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
            
            // 다른 모든 패널의 선택 상태를 해제
            foreach (var panel in allPanels)
            {
                panel.selectBackground.SetActive(panel == this);
            }
        }
    }
}
