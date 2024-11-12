using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Scripts.Utils;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "GameScene";

    public GameState CurrentGameState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        ChangeGameState(GameState.MainMenu);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            ChangeGameState(GameState.MainMenu);
        }
        else if (scene.name == gameSceneName)
        {
            ChangeGameState(GameState.UnitPlacement);
        }
    }
    
    public void StartNewGame()
    {
        StartCoroutine(LoadGameScene());
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator LoadGameScene()
    {
        // 여기에 로딩 화면 제작 들어가야 함
        yield return SceneManager.LoadSceneAsync(gameSceneName);
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMainMenuScene());
    }

    private IEnumerator LoadMainMenuScene()
    {
        yield return SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    public void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
        
        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuState();
                break;
            case GameState.UnitPlacement:
                HandleUnitPlacementState();
                break;
            case GameState.Playing:
                HandleGamePlayState();
                break;
            case GameState.Paused:
                HandlePausedState();
                break;
            case GameState.GameOver:
                HandleGameOverState();
                break;
        }
    }

    private void HandleMainMenuState()
    {
        Time.timeScale = 1f;
        // 메인 메뉴 UI 초기화 등
    }

    private void HandleUnitPlacementState()
    {
        Time.timeScale = 1f;
        EnableUnitPlacement(true);
        UpdateUIForPlacement();
    }

    private void HandleGamePlayState()
    {
        Time.timeScale = 1f;
        EnableUnitPlacement(false);
        UpdateUIForGameplay();
        StartGameLogic();
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0f;
        ShowPauseMenu(true);
    }

    private void HandleGameOverState()
    {
        Time.timeScale = 0f;
        ShowGameOverScreen();
        // 일정 시간 후 메인 메뉴로 돌아가기
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 게임 오버 화면을 보여주는 시간
        yield return new WaitForSecondsRealtime(3f);
        ReturnToMainMenu();
    }

    private void EnableUnitPlacement(bool enable)
    {
        // 유닛 배치 시스템 활성화/비활성화
        if (enable)
        {
            // 유닛 배치 UI 활성화
            // 유닛 배치 가능 영역 표시
            // 시작 버튼 활성화
        }
        else
        {
            // 유닛 배치 UI 비활성화
            // 배치 가능 영역 숨기기
            // 시작 버튼 비활성화
        }
    }

    private void UpdateUIForPlacement()
    {
        // 유닛 배치 UI 업데이트
        // - 배치 가능한 유닛 목록 표시
        // - 리소스/코스트 표시
        // - 시작 버튼 표시
    }

    private void UpdateUIForGameplay()
    {
        // 게임 플레이 UI 업데이트
        // - 게임 진행 상태 표시
        // - 스코어/시간 표시
        // - 일시정지 버튼 표시
    }

    private void ShowPauseMenu(bool show)
    {
        // 일시정지 메뉴 표시/숨김
        // - 계속하기
        // - 메인 메뉴로 돌아가기
        // - 다시시작
    }

    private void ShowGameOverScreen()
    {
        // 게임 오버 화면 표시
        // - 최종 스코어
        // - 게임 결과
        // - "메인 메뉴로" 버튼
    }

    private void StartGameLogic()
    {
        // 게임 시작 시 초기화
        // - 웨이브 시스템 시작
        // - 적 스폰 시작
        // - 게임 타이머 시작
    }

    // 게임 상태 확인 헬퍼 메서드
    public bool IsMainMenu() => CurrentGameState == GameState.MainMenu;
    public bool IsUnitPlacementPhase() => CurrentGameState == GameState.UnitPlacement;
    public bool IsGameRunning() => CurrentGameState == GameState.Playing;
    public bool IsGamePaused() => CurrentGameState == GameState.Paused;
    public bool IsGameOver() => CurrentGameState == GameState.GameOver;
}