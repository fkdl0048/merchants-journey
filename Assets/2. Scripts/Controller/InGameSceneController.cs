using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;

namespace _2._Scripts.Controller
{
    public class InGameSceneController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private GameUI gameUI;
        [SerializeField] private AudioClip gameBGM;
    
        private void Start()
        {
            InitializeGame();
        }
    
        private void InitializeGame()
        {
            GameManager.Instance.ChangeGameState(GameState.InGame);
            
            if (gameUI != null)
            {
                gameUI.Show();
            }
            
            AudioManager.Instance.PlayBGM(gameBGM);
        }
    
        private void Update()
        {
            // 인게임 키 입력
            // EX 설정창 등
        }
        
    }
}