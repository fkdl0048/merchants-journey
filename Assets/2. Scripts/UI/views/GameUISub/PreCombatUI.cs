using System;
using Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI.GameUISub
{
    public class PreCombatUI : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private GameObject requestUI;
        [SerializeField] private GameObject preCombatUI;
        [SerializeField] private Button requestNextButton;
        [SerializeField] private Button preCombatPrevButton;
        [SerializeField] private Button ExitButton;
        [SerializeField] private Button startStageButton;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI FamilyNameText;
        [SerializeField] private TextMeshProUGUI ClientNameText;
        [SerializeField] private TextMeshProUGUI RequestDescriptionText;
        [SerializeField] private TextMeshProUGUI TerrainInfoText;
        [SerializeField] private TextMeshProUGUI DangerFactionText;
        [SerializeField] private TextMeshProUGUI DepartureText;
        [SerializeField] private TextMeshProUGUI DestinationText;
        [SerializeField] private TextMeshProUGUI CargoTypeText;
        [SerializeField] private TextMeshProUGUI RewardCurrencyText;
        
        public UnityAction OnStageButtonClicked;
        public UnityAction OnExitClicked;

        // 1회 초기화
        private void Awake()
        {
            requestNextButton.onClick.AddListener(OnRequestNextButtonClicked);
            preCombatPrevButton.onClick.AddListener(OnPreCombatPrevButtonClicked);
            startStageButton.onClick.AddListener(OnStartStageButtonClicked);
            ExitButton.onClick.AddListener(OnExitButtonClicked);
            
            //Initialized();
        }
        
        // 호출 초기화
        public void Initialized(StageData stageData)
        {
            requestUI.SetActive(true);
            preCombatUI.SetActive(false);
            
            FamilyNameText.text = stageData.FamilyName;
            ClientNameText.text = stageData.ClientName;
            RequestDescriptionText.text = stageData.RequestDescription;
            TerrainInfoText.text = stageData.TerrainInfo;
            DangerFactionText.text = stageData.DangerFaction;
            DepartureText.text = stageData.Departure;
            DestinationText.text = stageData.Destination;
            CargoTypeText.text = stageData.CargoType;
            RewardCurrencyText.text = stageData.RewardCurrency.ToString();
            
            // requestUI 초기화 (데이터 불러오기)
            // state정보에 따라서..
        }
        
        // 다음 버튼 클릭
        private void OnRequestNextButtonClicked()
        {
            requestUI.SetActive(false);
            preCombatUI.SetActive(true);
        }
        
        // 이전 버튼 클릭
        private void OnPreCombatPrevButtonClicked()
        {
            requestUI.SetActive(true);
            preCombatUI.SetActive(false);
        }
        
        // 스테이지 시작 버튼 클릭
        private void OnStartStageButtonClicked()
        {
            OnStageButtonClicked?.Invoke();
        }
        
        private void OnExitButtonClicked()
        {
            OnExitClicked?.Invoke();
        }
    }
}