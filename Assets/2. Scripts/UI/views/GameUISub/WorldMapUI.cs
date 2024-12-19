using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [Header("World Map UI References")]
    [SerializeField] private Button UpgradesButton;
    [SerializeField] private Button NextStageButton;

    private void Awake()
    {
        UpgradesButton.onClick.AddListener(() =>
        {
            Debug.Log("Upgrade Button Clicked");
        });

        NextStageButton.onClick.AddListener(() =>
        {
            Debug.Log("Next Stage Button Clicked");
        });
    }
}
