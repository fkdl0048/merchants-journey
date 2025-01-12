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

        public int CurrentStageNumber { get; set; }
        private StageData currentStageData;
        private GameObject currentStageInstance;

        private void LoadCurrentStage()
        {
            if (stageDatas == null || stageDatas.Length == 0)
            {
                Debug.LogError("No stage data assigned to StageSystem!");
                return;
            }

            currentStageData = stageDatas.FirstOrDefault(x => x.stageNumber == CurrentStageNumber);
            
            if (currentStageData == null)
            {
                Debug.LogError($"Stage {CurrentStageNumber} data not found!");
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
        public CargoBehavior GetCargoBehavior()
        {
            return currentStageInstance.GetComponentInChildren<CargoBehavior>();
        }
        public void LoadStage()
        {
            LoadCurrentStage();
        }
        
        public void ClearStage()
        {
            Destroy(currentStageInstance);
        }
    }
}
