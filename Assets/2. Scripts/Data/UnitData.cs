using Scripts.Utils;
using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Game/UnitData")]
    public class UnitData : ScriptableObject
    {
        public UnitType unitType;
        public int cost;
        public Sprite icon;
        public GameObject prefab;
    }
}