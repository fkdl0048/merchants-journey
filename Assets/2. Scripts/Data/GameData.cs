using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public int currentStage = 1;
        public List<int> unlockedStages = new List<int>();
        public Dictionary<int, int> stageScores = new Dictionary<int, int>();
    }
}
