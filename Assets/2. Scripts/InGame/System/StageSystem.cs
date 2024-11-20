using System.Linq;
using UnityEngine;
using Scripts.Data;
using Scripts.Manager;

namespace Scripts.InGame.System
{
    public class StageSystem : MonoBehaviour
    {
        [SerializeField] private StageData[] stageDatas;  // Unity Inspector에서 설정
        [SerializeField] private Transform stageParent;   // 스테이지 프리팹이 생성될 부모 Transform

        private StageData currentStageData;
        private GameObject currentStageInstance;

        private void LoadCurrentStage()
        {
            // 디버그: stageDatas 배열 체크
            if (stageDatas == null || stageDatas.Length == 0)
            {
                Debug.LogError("No stage data assigned to StageSystem!");
                return;
            }

            // 저장된 게임 데이터에서 현재 스테이지 번호를 가져옴
            var gameData = SaveManager.Instance.GetGameData();
            int currentStageNumber = gameData?.currentStage ?? 1;  // 데이터가 없으면 1스테이지부터 시작

            Debug.Log($"Loading stage number: {currentStageNumber}");
            Debug.Log($"Available stages: {string.Join(", ", stageDatas.Select(x => x.stageNumber))}");

            // 현재 스테이지 데이터 찾기
            currentStageData = stageDatas.FirstOrDefault(x => x.stageNumber == currentStageNumber);
            
            if (currentStageData == null)
            {
                Debug.LogError($"Stage {currentStageNumber} data not found!");
                return;
            }

            // 이전 스테이지가 있다면 제거
            if (currentStageInstance != null)
            {
                Destroy(currentStageInstance);
            }

            // 새 스테이지 생성
            if (currentStageData.stagePrefab == null)
            {
                Debug.LogError($"Stage {currentStageNumber} prefab is missing!");
                return;
            }

            currentStageInstance = Instantiate(currentStageData.stagePrefab, stageParent);
            Debug.Log($"Stage {currentStageData.stageNumber} loaded successfully!");
        }

        public StageData GetCurrentStageData()
        {
            return currentStageData;
        }

        public void LoadStage()
        {
            LoadCurrentStage();
        }
    }
}
