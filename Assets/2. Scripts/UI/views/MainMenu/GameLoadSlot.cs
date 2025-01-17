using System;
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
        TimeSpan timeSinceLastSave = DateTime.Now - gameData.lastSaveTime;
        string daysAgo = timeSinceLastSave.Days > 0 ? $"{timeSinceLastSave.Days}일 전" : "오늘";
        
        string playTimeStr;
        try 
        {
            TimeSpan playTimeSpan = TimeSpan.FromSeconds(Math.Min(gameData.playTime, TimeSpan.MaxValue.TotalSeconds));
            playTimeStr = $"{playTimeSpan.Hours:D2}:{playTimeSpan.Minutes:D2}:{playTimeSpan.Seconds:D2}";
        }
        catch (Exception)
        {
            playTimeStr = "99:59:59"; // 최대값으로 표시
        }
        
        gamePlayInfoText.text = $"플레이 타임: {playTimeStr} {daysAgo}";
        
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.LoadGameFromSlot(slotIndex);  // LoadGameFromSlot 메서드 사용
            LoadingSceneController.LoadScene(Consts.InGameSceneName);
        });
        
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() =>
        {
            ButtonPopup.Show("저장된 게임을 삭제하시겠습니까?",
                () =>
                {
                    SaveManager.Instance.DeleteGameDataFromSlot(slotIndex);
                    InitNewGame();
                });
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
