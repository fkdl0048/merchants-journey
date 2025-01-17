using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadUI : MonoBehaviour
{
    [Header("GameLoadSlot Prefab")]
    [SerializeField] private GameLoadSlot gameLoadSlotPrefab;
    [SerializeField] private GameObject gameLoadSlotContainer;
    [SerializeField] private int maxGameSlotCount = 5;
    
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        Init();
        
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(OnCloseButtonClick);
    }
    
    public void Init()
    {
        // Clear existing slots
        foreach (Transform child in gameLoadSlotContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        for (int i = 0; i < maxGameSlotCount; i++)
        {
            GameLoadSlot gameLoadSlot = Instantiate(gameLoadSlotPrefab, gameLoadSlotContainer.transform);
            gameLoadSlot.Init(i);
        }
    }

    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }
}
