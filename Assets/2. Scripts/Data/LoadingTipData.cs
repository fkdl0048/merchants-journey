using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "LoadingTipData", menuName = "Game/Loading Tip Data")]
    public class LoadingTipData : ScriptableObject
    {
        public List<string> tips = new List<string>();

#if UNITY_EDITOR
        public void ImportFromCSV(TextAsset csvFile)
        {
            tips.Clear();
            string[] lines = csvFile.text.Split('\n');
            
            // Skip header if exists and process each line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    tips.Add(line);
                }
            }
        }
#endif
    }
}
