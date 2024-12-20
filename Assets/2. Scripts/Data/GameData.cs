using System;
using System.Collections.Generic;
using _2._Scripts.Unit;

namespace Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public int currentStage = 1;
        public int gold = 1000; // 초기 골드 설정
        public List<UnitData> ownedUnits = new List<UnitData>();

        public bool HasEnoughResources(int cost)
        {
            return gold >= cost;
        }

        public void ConsumeResources(int cost)
        {
            if (HasEnoughResources(cost))
            {
                gold -= cost;
            }
        }

        public void AddGold(int amount)
        {
            gold += amount;
        }

        public int GetGold()
        {
            return gold;
        }
    }
}
