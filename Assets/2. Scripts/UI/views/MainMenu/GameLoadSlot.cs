using System.Collections;
using System.Collections.Generic;
using Scripts.Controller;
using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
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

    private int slotIndex;

    public void Init(int index)
    {
        slotIndex = index;
        gameIndexText.text = $"Game {index + 1}";
        
        bool hasSavedGame = SaveManager.Instance.HasSavedGame(index);
        if (hasSavedGame)
        {
            InitLoadGame();
        }
        else
        {
            InitNewGame();
        }
    }

    private void InitLoadGame()
    {
        newGameButton.gameObject.SetActive(false);
        
        var gameData = SaveManager.Instance.LoadGameDataBySlot(slotIndex);
        gamePlayInfoText.text = $"Stage {gameData.currentStage}\nGold: {gameData.gold}";
        
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() =>
        {
            var loadedData = SaveManager.Instance.LoadGameDataBySlot(slotIndex);
            SaveManager.Instance.SaveGameData(loadedData);  // 현재 게임 데이터로 설정
            LoadingSceneController.LoadScene(Consts.InGameSceneName);
        });
        
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.DeleteGameDataFromSlot(slotIndex);
            InitNewGame();
        });
    }

    private void InitNewGame()
    {
        newGameButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        
        gamePlayInfoText.text = "신규 게임";
        
        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.CreateAndSaveNewGame(slotIndex);
            LoadingSceneController.LoadScene(Consts.InGameSceneName);
        });
    }
}
