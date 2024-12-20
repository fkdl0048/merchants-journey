using System;
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
        
        public UnityAction OnStageButtonClicked;
        public UnityAction OnExitClicked;

        // 1회 초기화
        private void Awake()
        {
            requestNextButton.onClick.AddListener(OnRequestNextButtonClicked);
            preCombatPrevButton.onClick.AddListener(OnPreCombatPrevButtonClicked);
            startStageButton.onClick.AddListener(OnStartStageButtonClicked);
            ExitButton.onClick.AddListener(OnExitButtonClicked);
            
            Initialized();
        }
        
        // 호출 초기화
        public void Initialized()
        {
            requestUI.SetActive(true);
            preCombatUI.SetActive(false);
            
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