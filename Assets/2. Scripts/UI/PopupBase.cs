using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public abstract class PopupBase : UIBase
    {
        [SerializeField] protected Button closeButton;
        
        protected override void Initialize()
        {
            base.Initialize();
            SetupCloseButton();
        }
        
        private void SetupCloseButton()
        {
            if (closeButton == null)
                closeButton = transform.Find("CloseButton")?.GetComponent<Button>();
                
            if (closeButton != null)
                closeButton.onClick.AddListener(OnClose);
        }
        
        protected virtual void OnClose()
        {
            UIManager.Instance.ClosePopup(this);
        }
        
        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveAllListeners();
        }
    }
}