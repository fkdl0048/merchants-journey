using Scripts.UI;
using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ButtonPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    
    private UnityAction onYesCallback;
    private UnityAction onNoCallback;

    public void Initialize(string title, UnityAction yesCallback, UnityAction noCallback = null)
    {
        titleText.text = title;
        
        onYesCallback = yesCallback;
        onNoCallback = noCallback;

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => 
        {
            AudioManager.Instance.PlaySFX("Audio/SFX/Button");
            onYesCallback?.Invoke();
            ClosePopup();
        });

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => 
        {
            AudioManager.Instance.PlaySFX("Audio/SFX/Button");
            onNoCallback?.Invoke();
            ClosePopup();
        });
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopup(this);
    }

    public static ButtonPopup Show(string title, UnityAction yesCallback, UnityAction noCallback = null)
    {
        var popup = UIManager.Instance.ShowPopup<ButtonPopup>("UI/Popups/ButtonPopup");
        if (popup != null)
        {
            popup.Initialize(title, yesCallback, noCallback);
        }
        return popup;
    }
}
