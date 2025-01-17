using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadUI : MonoBehaviour
{
    [Header("GameLoadSlot Prefab")]
    [SerializeField] private GameLoadSlot gameLoadSlotPrefab;
    [SerializeField] private GameObject gameLoadSlotContainer;
    [SerializeField] private int maxGameSlotCount = 5;
    
    public void Init()
    {
        for (int i = 0; i < maxGameSlotCount; i++)
        {
            GameLoadSlot gameLoadSlot = Instantiate(gameLoadSlotPrefab, gameLoadSlotContainer.transform);
            gameLoadSlot.Init(i);
        }
    }
}
