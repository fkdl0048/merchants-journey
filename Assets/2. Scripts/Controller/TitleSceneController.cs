using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;

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
