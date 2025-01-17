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
    /// <summary>
    /// InGameSceneController는 InGame 씬의 전반적인 게임 흐름을 관리하는 컨트롤러입니다.
    /// GameManager의 State는 게임 전반 (로비, 메인 등)
    /// 여기서는 IngGame의 상태만 관리 (State Pattern)
    /// 규칙 1. ChageState는 해당 상태에서 자신이 자신의 상태를 Enter에서 호출 모든 State동일 규칙
    /// </summary>
    public class InGameSceneController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private GameUI gameUI;
        [SerializeField] private UnitSystem unitSystem;
        [SerializeField] private StageSystem stageSystem;
        [SerializeField] private ClickSystem clickSystem;
        [SerializeField] private AudioClip gameBGM;
        
        private Dictionary<InGameState, IInGameState> states;
        private IInGameState currentState;
        private OptionsPopup optionsPopup;
        private bool isOptionsPopupOpen = false;

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
                    InGameState.WorldMap,
                    new WorldMapState(this, gameUI, stageSystem)
                },
                {
                    InGameState.PreCombat,
                    new PreCombatState(this, stageSystem, gameUI)
                },
                {
                    InGameState.Upgrade,
                    new UpgradeState(this, gameUI, unitSystem)
                },
                {
                    InGameState.UnitPlacement,
                    new UnitPlacementState(this, gameUI, unitSystem, stageSystem)
                },
                {
                    InGameState.Wave,
                    new BattleState(this, gameUI, unitSystem, stageSystem, clickSystem)
                },
                {
                    InGameState.StageClear,
                    new StageClearState(this, gameUI, stageSystem)
                },
                {
                    InGameState.StageOver,
                    new StageOverState(this, gameUI)
                },
                // stage fail state 추가..예정
            };
        }

        private void InitializeGame()
        {
            GameManager.Instance.ChangeGameState(GameState.InGame);
        
            // BGM 재생
            AudioManager.Instance.PlayBGM(gameBGM);
        
            // 초기 상태 설정
            ChangeInGameState(InGameState.WorldMap);
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
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleOptionsPopup();
            }
        }

        private void ToggleOptionsPopup()
        {
            if (isOptionsPopupOpen)
            {
                if (optionsPopup != null)
                {
                    optionsPopup.OnClose();
                    optionsPopup = null;
                }
                isOptionsPopupOpen = false;
            }
            else
            {
                optionsPopup = OptionsPopup.Show();
                isOptionsPopupOpen = true;
            }
        }

        private void OnDestroy()
        {
            if (this.gameObject.scene.isLoaded)
            {
                currentState?.Exit();
            }
        }
        
    }
}