using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSceneController : MonoBehaviour
{    
    void Start()
    {
        InitializeManagers();
        LoadTitleScene();
    }
    
    private void InitializeManagers()
    {
        GameManager.Instance.ChangeGameState(GameState.Boot);
        
        var savedData = SaveManager.Instance.LoadData<GameSettings>("GameSettings");
        AudioManager.Instance.SetBGMVolume(savedData.bgmVolume);
        AudioManager.Instance.SetSFXVolume(savedData.sfxVolume);
    }
    
    private void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}
