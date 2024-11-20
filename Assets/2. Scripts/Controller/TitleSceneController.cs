using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;

/// <summary>
/// TitleSceneController는 메인 메뉴 담당
/// 이후에 mainMenuUI와 연결 지점 (디커플링)
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private MainMenuUI mainMenuUI;
    
    private void Start()
    {
        GameManager.Instance.ChangeGameState(GameState.Title);
        mainMenuUI.Show();
        AudioManager.Instance.PlayBGM(titleBGM);
    }
}
