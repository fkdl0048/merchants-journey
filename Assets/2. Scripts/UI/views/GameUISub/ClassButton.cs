using UnityEngine;
using UnityEngine.UI;
using Scripts.Utils;

namespace Scripts.UI.GameUISub
{
    public class ClassButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        private UnitClass classType;
        private System.Action<UnitClass> onClassSelected;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        public void Initialize(UnitClass type, System.Action<UnitClass> callback)
        {
            classType = type;
            onClassSelected = callback;
        }

        private void OnButtonClicked()
        {
            onClassSelected?.Invoke(classType);
        }
    }
}
