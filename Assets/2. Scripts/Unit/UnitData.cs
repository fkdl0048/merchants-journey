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
        public int moveSpeedCount;
        public int attackDamageCount;
        public int defenseCount;

        public UnitData(string unitId, string unitName, UnitType unitType, UnitClass unitClass)
        {
            this.unitId = unitId;
            this.unitName = unitName;
            this.unitType = unitType;
            this.unitClass = unitClass;
            
            this.skillType = SkillType.None;
            this.moveSpeedCount = 2;
            this.attackDamageCount = 1;
            this.defenseCount = 1;
        }
    }
}