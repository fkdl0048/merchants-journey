using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
    public class StageData : ScriptableObject
    {
        public int stageNumber;
        public GameObject stagePrefab;
        public Vector2[] spawnPoints;  // 유닛을 배치할 수 있는 위치들
        public Vector2 cargoPoint;     // 화물이 위치할 지점
    }
}
