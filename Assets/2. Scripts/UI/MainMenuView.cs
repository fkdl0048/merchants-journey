using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private MainMenuViewModel viewModel;

        private void Start()
        {
            viewModel = new MainMenuViewModel();
        
            // 데이터 바인딩
            startButton.onClick.AddListener(() => viewModel.StartGameCommand.Execute(null));
            quitButton.onClick.AddListener(() => viewModel.QuitGameCommand.Execute(null));

            // ViewModel 업데이트 구독
           // viewModel.PropertyChanged += HandlePropertyChanged;
        }
    }
}