using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Collections;
using Scripts.Data;
using Scripts.Manager;
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

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Dependencies")]
    [SerializeField] private GameSettings gameSettings;

    public GameState CurrentGameState { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    // 게임 진행 관련
    private bool isGameInitialized;
    private bool isLoadingScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeGame();
    }

    private void InitializeGame()
    {
        if (isGameInitialized) return;

        // 필수 매니저들이 모두 준비되었는지 확인
        ValidateManagers();
        
        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // 게임 설정 적용
        ApplyGameSettings();

        isGameInitialized = true;
    }

    private void ValidateManagers()
    {
        // 필수 매니저들이 없다면 생성
        if (FindObjectOfType<UIManager>() == null)
        {
            GameObject uiManagerGO = new GameObject("UIManager");
            uiManagerGO.AddComponent<UIManager>();
            DontDestroyOnLoad(uiManagerGO);
        }

        if (FindObjectOfType<DataManager>() == null)
        {
            GameObject dataManagerGO = new GameObject("DataManager");
            dataManagerGO.AddComponent<DataManager>();
            DontDestroyOnLoad(dataManagerGO);
        }
    }

    private void ApplyGameSettings()
    {
        if (gameSettings != null)
        {
            Application.targetFrameRate = gameSettings.targetFrameRate;
            // 다른 게임 설정들 적용
        }
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public async void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        GameState previousState = CurrentGameState;
        CurrentGameState = newState;

        try
        {
            // 이전 상태 정리
            await CleanupState(previousState);

            // 새로운 상태 설정
            await SetupState(newState);

            // 상태 변경 이벤트 발생
            OnGameStateChanged?.Invoke(newState);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error changing game state: {e.Message}");
            // 에러 복구 로직
        }
    }

    private async Task CleanupState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                // 진행 중이던 게임 로직 정리
                await SaveGameProgress();
                break;
        }
    }

    private async Task SetupState(GameState state)
    {
        switch (state)
        {
            case GameState.UnitPlacement:
                await InitializeUnitPlacement();
                break;
            case GameState.Playing:
                await StartGameplay();
                break;
            case GameState.GameOver:
                await HandleGameOver();
                break;
        }
    }

    public async void StartNewGame()
    {
        if (isLoadingScene) return;
        isLoadingScene = true;

        try
        {
            // 기존 게임 데이터 초기화
            DataManager.Instance.ResetGameData();
            
            // 게임 씬 로드
            await LoadSceneAsync(gameSceneName);
        }
        finally
        {
            isLoadingScene = false;
        }
    }

    public async void ReturnToMainMenu()
    {
        if (isLoadingScene) return;
        isLoadingScene = true;

        try
        {
            await LoadSceneAsync(mainMenuSceneName);
        }
        finally
        {
            isLoadingScene = false;
        }
    }

    private async Task LoadSceneAsync(string sceneName)
    {
        // UI 매니저에게 로딩 화면 표시 요청
        UIManager.Instance.ShowLoadingScreen(true);

        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            await Task.Yield();
            // 로딩 진행률 업데이트
            UIManager.Instance.UpdateLoadingProgress(operation.progress);
        }

        // 로딩 완료 후 잠시 대기 (너무 빨리 지나가는 것 방지)
        await Task.Delay(500);
        
        operation.allowSceneActivation = true;
        
        // 씬 전환 완료 대기
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        UIManager.Instance.ShowLoadingScreen(false);
    }

    private async Task InitializeUnitPlacement()
    {
        // 유닛 배치 모드 초기화
        // DataManager.Instance.Currency = gameSettings.initialCurrency;
        await Task.CompletedTask;
    }

    private async Task StartGameplay()
    {
        // 실제 게임 로직 시작
        Time.timeScale = 1f;
        await Task.CompletedTask;
    }

    private async Task HandleGameOver()
    {
        Time.timeScale = 0f;
        
        // 최종 점수 계산 및 저장
        int finalScore = DataManager.Instance.CurrentScore;
        await SaveHighScore(finalScore);
    }

    private async Task SaveGameProgress()
    {
        // 게임 진행 상황 저장
        await Task.CompletedTask;
    }

    private async Task SaveHighScore(int score)
    {
        // 최고 점수 저장
        await Task.CompletedTask;
    }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // 게임 상태 확인 헬퍼 메서드
    public bool IsMainMenu() => CurrentGameState == GameState.MainMenu;
    public bool IsUnitPlacementPhase() => CurrentGameState == GameState.UnitPlacement;
    public bool IsGameRunning() => CurrentGameState == GameState.Playing;
    public bool IsGamePaused() => CurrentGameState == GameState.Paused;
    public bool IsGameOver() => CurrentGameState == GameState.GameOver;
}