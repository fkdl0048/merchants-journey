using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
    public class StageData : ScriptableObject
    {
        public int stageNumber;
        public GameObject stagePrefab;
        // 이후에 추가되는 컨텐츠 등등
    }
}
