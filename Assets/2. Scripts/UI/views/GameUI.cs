using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeText;
    
        protected override void Initialize()
        {
            base.Initialize();
            UpdateUI();
        }
    
        private void UpdateUI()
        {
            // 게임 상태에 따른 UI 업데이트
            if (scoreText != null)
                scoreText.text = "Score: 0";
            
            if (timeText != null)
                timeText.text = "Time: 0:00";
        }
    }
}