using System.ComponentModel;
using Scripts.Interface;
using Scripts.Utils;

namespace Scripts.UI
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        // 컨벤션을 위해, 등록할 이벤트 없음 (메인메뉴 알파 기준)
        public event PropertyChangedEventHandler PropertyChanged; 

        public ICommand StartGameCommand { get; }
        public ICommand QuitGameCommand { get; }

        public MainMenuViewModel()
        {
            StartGameCommand = new RelayCommand(StartGame);
            QuitGameCommand = new RelayCommand(QuitGame);
        }

        private void StartGame()
        {
            GameManager.Instance.StartNewGame();
        }

        private void QuitGame()
        {
            GameManager.Instance.QuitGame();
        }
    }
}