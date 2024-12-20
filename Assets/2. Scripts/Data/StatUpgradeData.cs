using UnityEngine;
using System.Collections.Generic;
using Scripts.Utils;

namespace Scripts.Data
{
    [System.Serializable]
    public class StatUpgradeInfo
    {
        public UnitType UnitType;
        public StatType StatType;
        public int MinValue = 1;
        public int MaxValue = 5;
        public int InitialValue;
        public int UpgradeCost;
        public string Description;
    }

    [CreateAssetMenu(fileName = "StatUpgradeData", menuName = "Game/Stat Upgrade Data")]
    public class StatUpgradeData : ScriptableObject
    {
        public List<StatUpgradeInfo> statUpgrades = new List<StatUpgradeInfo>();

#if UNITY_EDITOR
        public void ImportFromCSV(TextAsset csvFile)
        {
            statUpgrades.Clear();
            string[] lines = csvFile.text.Split('\n');
            
            // Skip header
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = line.Split(',');
                if (values.Length >= 7)
                {
                    StatUpgradeInfo info = new StatUpgradeInfo
                    {
                        UnitType = (UnitType)System.Enum.Parse(typeof(UnitType), values[0].Trim()),
                        StatType = (StatType)System.Enum.Parse(typeof(StatType), values[1].Trim()),
                        MinValue = int.Parse(values[2].Trim()),
                        MaxValue = int.Parse(values[3].Trim()),
                        InitialValue = int.Parse(values[4].Trim()),
                        UpgradeCost = int.Parse(values[5].Trim()),
                        Description = values[6].Trim()
                    };
                    statUpgrades.Add(info);
                }
            }
        }
#endif
    }
}
