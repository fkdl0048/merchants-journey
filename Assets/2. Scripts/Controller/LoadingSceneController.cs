using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Scripts.Utils;
using UnityEngine.SceneManagement;
using Scripts.Data;

namespace Scripts.Controller
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private float DotAnimationInterval = 0.3f;
        [SerializeField] private LoadingTipData tipData;

        private string nextSceneName;
        private float minimumLoadingTime = 1f;

        private void Start()
        {
            SetRandomTip();
            //StartCoroutine(LoadingTextAnimation());
            StartCoroutine(LoadSceneAsync());
        }

        private void SetRandomTip()
        {
            if (tipData != null && tipData.tips.Count > 0)
            {
                tipText.text = tipData.tips[Random.Range(0, tipData.tips.Count)];
            }
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
