using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ButtonPopup : MonoBehaviour
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
            onYesCallback?.Invoke();
            ClosePopup();
        });

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => 
        {
            onNoCallback?.Invoke();
            ClosePopup();
        });
    }

    private void ClosePopup()
    {
        Destroy(gameObject);
    }

    public static ButtonPopup Show(string title, UnityAction yesCallback, UnityAction noCallback = null)
    {
        // Popup prefab should be located in Resources/Prefabs/UI/Popups/ButtonPopup
        var prefab = Resources.Load<ButtonPopup>("UI/Popups/ButtonPopup");
        var instance = Instantiate(prefab);
        instance.Initialize(title, yesCallback, noCallback);
        return instance;
    }
}
