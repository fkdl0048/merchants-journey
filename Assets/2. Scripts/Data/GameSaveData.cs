using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public class GameSaveData
    {
        public int score;
        public int currency;
        public List<string> placedUnitIds = new List<string>();
    }
}