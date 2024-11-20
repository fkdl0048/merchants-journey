using System.Collections.Generic;
using Scripts.InGame.State;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Controller
{
    public class InGameSceneController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private GameUI gameUI;
        [SerializeField] private UnitSystem unitSystem;
        [SerializeField] private StageSystem stageController;
        [SerializeField] private AudioClip gameBGM;
        
        private Dictionary<InGameState, IInGameState> states;
        private IInGameState currentState;

        private void Awake()
        {
            InitializeStates();
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeStates()
        {
            states = new Dictionary<InGameState, IInGameState>
            {
                {
                    InGameState.UnitPlacement,
                    new UnitPlacementState(this, gameUI, unitSystem, stageController)
                },
                {
                    InGameState.Wave,
                    new BattleState(this, gameUI, stageController)
                },
                {
                    InGameState.StageClear,
                    new StageClearState(this, gameUI)
                },
                {
                    InGameState.StageOver,
                    new StageOverState(this, gameUI)
                },
                // stage fail state
            };
        }

        private void InitializeGame()
        {
            GameManager.Instance.ChangeGameState(GameState.InGame);
        
            // BGM 재생
            AudioManager.Instance.PlayBGM(gameBGM);
        
            // 초기 상태 설정
            ChangeInGameState(InGameState.UnitPlacement);
        }
        
        public void ChangeInGameState(InGameState newState)
        {
            currentState?.Exit();
            
            currentState = states[newState];
            currentState.Enter();
            
            Debug.Log($"Game State Changed to: {newState}");
        }
    
        private void Update()
        {
            currentState?.Update();
        }

        private void OnDestroy()
        {
            if (this.gameObject.scene.isLoaded)  // 씬이 아직 로드되어 있을 때만 실행
            {
                currentState?.Exit();
            }
        }
        
    }
}