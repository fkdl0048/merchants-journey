using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.Manager
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private float DotAnimationInterval = 0.3f;
        
        // 나중에 엑셀로 교체
        private string[] tips = new string[]
        {
            "상인이 되어 여행을 떠나보세요!",
            "아이템을 사고팔아 이윤을 남겨보세요.",
            "각 마을마다 특산품이 다르답니다.",
            "여행 중에 만나는 NPC들과 대화해보세요.",
            "인벤토리 관리가 중요합니다!"
        };

        private string nextSceneName;
        private float minimumLoadingTime = 1f;

        private void Start()
        {
            // 랜덤 팁 선택
            tipText.text = tips[Random.Range(0, tips.Length)];
            StartCoroutine(LoadingTextAnimation());
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadingTextAnimation()
        {
            string baseText = "로딩";
            int dotCount = 0;
            
            while (true)
            {
                dotCount = (dotCount + 1) % 4;
                loadingText.text = baseText + new string('.', dotCount);
                yield return new WaitForSeconds(DotAnimationInterval);
            }
        }

        private IEnumerator LoadSceneAsync()
        {
            nextSceneName = PlayerPrefs.GetString("NextScene");
            float startTime = Time.time;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                float elapsedTime = Time.time - startTime;

                if (asyncOperation.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        public static void LoadScene(string sceneName)
        {
            PlayerPrefs.SetString("NextScene", sceneName);
            SceneManager.LoadScene(Consts.LoadingSceneName);
        }
    }
}
