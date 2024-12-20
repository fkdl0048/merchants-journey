using UnityEngine;
using UnityEditor;
using Scripts.Data;

namespace Scripts.Editor
{
    [CustomEditor(typeof(LoadingTipData))]
    public class LoadingTipDataEditor : UnityEditor.Editor
    {
        private TextAsset csvFile;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LoadingTipData tipData = (LoadingTipData)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("CSV Import", EditorStyles.boldLabel);
            
            csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

            if (GUILayout.Button("Import from CSV"))
            {
                if (csvFile != null)
                {
                    tipData.ImportFromCSV(csvFile);
                    EditorUtility.SetDirty(tipData);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please select a CSV file first!", "OK");
                }
            }
        }
    }
}
