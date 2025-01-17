using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadSlot : MonoBehaviour
{
    [Header("Load")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI gameIndexText;
    [SerializeField] private TextMeshProUGUI gamePlayInfoText;
    
    [Header("New Game")]
    [SerializeField] private Button newGameButton;

    public void Init(int index)
    {
        gameIndexText.text = $"Game {index + 1}";
        gamePlayInfoText.text = "Empty";
        
        loadButton.onClick.AddListener(() =>
        {
            Debug.Log($"Load Game {index + 1}");
        });
        
        deleteButton.onClick.AddListener(() =>
        {
            Debug.Log($"Delete Game {index + 1}");
        });
        
        newGameButton.onClick.AddListener(() =>
        {
            Debug.Log($"New Game {index + 1}");
        });
    }
    
}
