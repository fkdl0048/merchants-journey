using Scripts.Manager;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PopupBase : UIBase
    {
        protected Button closeButton;
        
        protected override void Awake()
        {
            base.Awake();
            SetupCloseButton();
        }
        
        private void SetupCloseButton()
        {
            closeButton = transform.Find("CloseButton")?.GetComponent<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => UIManager.Instance.ClosePopup(this));
            }
        }
        
        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
            }
        }
    }
}