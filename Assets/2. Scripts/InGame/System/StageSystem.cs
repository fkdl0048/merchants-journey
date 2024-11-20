using System.Linq;
using UnityEngine;
using Scripts.Data;
using Scripts.Manager;

namespace Scripts.InGame.System
{
    /// <summary>
    /// 스테이지 데이터를 관리하고 스테이지를 생성하는 시스템
    /// 각 State에 InGameSceneController를 넘겨주는 방식으로 작동 Dependency Injection
    /// </summary>
    public class StageSystem : MonoBehaviour
    {
        [SerializeField] private StageData[] stageDatas;
        [SerializeField] private Transform stageParent;

        private StageData currentStageData;
        private GameObject currentStageInstance;

        private void LoadCurrentStage()
        {
            if (stageDatas == null || stageDatas.Length == 0)
            {
                Debug.LogError("No stage data assigned to StageSystem!");
                return;
            }
            
            var gameData = SaveManager.Instance.GetGameData();
            int currentStageNumber = gameData?.currentStage ?? 1;  // 데이터가 없으면 1스테이지부터 시작

            currentStageData = stageDatas.FirstOrDefault(x => x.stageNumber == currentStageNumber);
            
            if (currentStageData == null)
            {
                Debug.LogError($"Stage {currentStageNumber} data not found!");
                return;
            }

            // 이전 스테이지가 있다면 제거하기 전에 UnitSystem에 알림
            if (currentStageInstance != null)
            {
                Destroy(currentStageInstance);
            }

            // 새 스테이지 생성
            if (currentStageData.stagePrefab == null)
            {
                return;
            }

            currentStageInstance = Instantiate(currentStageData.stagePrefab, stageParent);
        }

        // 뒤 데이터 나오면 사용 예정
        public StageData GetCurrentStageData()
        {
            return currentStageData;
        }
        
        public Cargo GetCargo()
        {
            return currentStageInstance.GetComponentInChildren<Cargo>();
        }

        public void LoadStage()
        {
            LoadCurrentStage();
        }
    }
}
