using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// boot scene에서 게임 매니저와 오디오 매니저를 초기화하고 타이틀 씬을 로드하는 클래스
/// 이후에 여기서 게임 시작 시 나오는 로고 등을 추가
/// </summary>
public class BootSceneController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    
    void Start()
    {
        InitializeManagers();
        startButton?.onClick.AddListener(LoadTitleScene);
    }
    
    private void InitializeManagers()
    {
        GameManager.Instance.ChangeGameState(GameState.Boot);
        
        var savedData = SaveManager.Instance.LoadData<GameSettings>("GameSettings");
        AudioManager.Instance.SetBGMVolume(savedData.bgmVolume);
        AudioManager.Instance.SetSFXVolume(savedData.sfxVolume);
        
        AudioManager.Instance.PlayBGM("Audio/BGM/Title");
    }
    
    private void LoadTitleScene()
    {
        SceneManager.LoadScene(Consts.TitleSceneName);
        
        AudioManager.Instance.PlaySFX("Audio/SFX/Button");
    }
}
