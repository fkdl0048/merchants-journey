using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
    public class StageData : ScriptableObject
    {
        public string Stage;
        
        // 가문명
        public string FamilyName;
        
        // 의뢰인
        public string ClientName;
        
        // 의뢰 내용
        [TextArea]
        public string RequestDescription;
        
        // 지형 정보
        public string TerrainInfo;
        
        // 위험 세력
        public string DangerFaction;
        
        // 출발지
        public string Departure;
        
        // 목적지
        public string Destination;
        
        // 화물 종류
        public string CargoType;
        
        // 획득 재화
        public int RewardCurrency;
        
        // Legacy fields
        public int stageNumber;
        public GameObject stagePrefab;
        // 이후에 추가되는 컨텐츠 등등
    }
}
