using Scripts.Utils;
using UnityEngine;

namespace _2._Scripts.Unit
{
    public class UnitData
    {
        [Header("Basic Info")]
        public string unitId;
        public string unitName;
        public UnitType unitType;
        public UnitClass unitClass;
        
        [Header("Upgrade Stats")]
        public SkillType skillType;
        public float attackPower;
        public float defense;
        public float moveSpeed;

        public UnitData(string unitId, string unitName, UnitType unitType, UnitClass unitClass)
        {
            this.unitId = unitId;
            this.unitName = unitName;
            this.unitType = unitType;
            this.unitClass = unitClass;
        }
    }
}