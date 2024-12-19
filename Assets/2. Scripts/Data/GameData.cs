using System;
using System.Collections.Generic;
using _2._Scripts.Unit;

namespace Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public int currentStage = 1;
        public int gold = 0;
        public List<UnitData> ownedUnits = new List<UnitData>();
    }
}
