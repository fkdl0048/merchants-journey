using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Cargo))]
public class CargoPathEditor : Editor
{
    private Cargo cargo;
    private bool isSettingPath = false;
    private SerializedProperty serializedPathPointsProp;
    private SerializedProperty moveSpeedProp;
    private SerializedProperty waitTimeAtPointProp;
    private SerializedProperty placementRangeProp;
    private SerializedProperty autoStartProp;

    private void OnEnable()
    {
        cargo = (Cargo)target;
        serializedPathPointsProp = serializedObject.FindProperty("serializedPathPoints");
        moveSpeedProp = serializedObject.FindProperty("moveSpeed");
        waitTimeAtPointProp = serializedObject.FindProperty("waitTimeAtPoint");
        placementRangeProp = serializedObject.FindProperty("placementRange");
        autoStartProp = serializedObject.FindProperty("autoStart");
        
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(moveSpeedProp);
        EditorGUILayout.PropertyField(waitTimeAtPointProp);
        EditorGUILayout.PropertyField(placementRangeProp);
        EditorGUILayout.PropertyField(autoStartProp);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Path Settings", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(serializedPathPointsProp);

        EditorGUILayout.Space();

        GUI.backgroundColor = isSettingPath ? Color.red : Color.green;
        if (GUILayout.Button(isSettingPath ? "Stop Setting Path" : "Start Setting Path"))
        {
            isSettingPath = !isSettingPath;
            if (!isSettingPath)
            {
                Tools.current = Tool.Move;
            }
        }
        GUI.backgroundColor = Color.white;

        if (isSettingPath)
        {
            EditorGUILayout.HelpBox(
                "Click on tiles to add path points.\n" +
                "Hold Ctrl + Click to remove the last point.\n" +
                "Click 'Stop Setting Path' when finished.", 
                MessageType.Info);
        }

        if (GUILayout.Button("Clear Path"))
        {
            if (EditorUtility.DisplayDialog("Clear Path", 
                "Are you sure you want to clear the entire path?", 
                "Yes", "No"))
            {
                Undo.RecordObject(cargo, "Clear Path");
                cargo.ClearPath();
                EditorUtility.SetDirty(cargo);
            }
        }

        EditorGUILayout.Space();
        
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Start Moving"))
            {
                cargo.StartMoving();
            }

            if (GUILayout.Button("Stop Moving"))
            {
                cargo.StopMoving();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isSettingPath) return;

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                Transform current = hit.transform;
                Tile tile = null;
                
                while (current != null)
                {
                    tile = current.GetComponent<Tile>();
                    if (tile != null) break;
                    current = current.parent;
                }

                if (tile != null && tile.isWalkable)
                {
                    if (e.control)
                    {
                        if (cargo.pathPoints.Count > 0)
                        {
                            Undo.RecordObject(cargo, "Remove Path Point");
                            cargo.RemovePathPoint(cargo.pathPoints.Count - 1);
                            EditorUtility.SetDirty(cargo);
                        }
                    }
                    else
                    {
                        Vector3 tileCenter = current.position;
                        bool isDuplicate = cargo.pathPoints.Exists(p => 
                            Vector3.Distance(p, tileCenter) < 0.1f);
                            
                        if (!isDuplicate)
                        {
                            Undo.RecordObject(cargo, "Add Path Point");
                            cargo.AddPathPoint(tileCenter);
                            EditorUtility.SetDirty(cargo);
                        }
                    }
                    
                    e.Use();
                    break;
                }
            }
        }
        
        DrawPathVisuals();
        sceneView.Repaint();
    }

    private void DrawPathVisuals()
    {
        List<Vector3> points = cargo.pathPoints;
        if (points.Count == 0) return;

        // 시작점 표시
        Handles.color = Color.green;
        Handles.SphereHandleCap(
            0,
            points[0],
            Quaternion.identity,
            0.3f,
            EventType.Repaint
        );
        
        // 경로 그리기
        Handles.color = Color.yellow;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Handles.DrawLine(points[i], points[i + 1]);
            
            // 중간 포인트
            if (i < points.Count - 2)
            {
                Handles.SphereHandleCap(
                    0,
                    points[i + 1],
                    Quaternion.identity,
                    0.1f,
                    EventType.Repaint
                );
            }
        }
        
        // 도착점 표시
        if (points.Count > 1)
        {
            Handles.color = Color.red;
            Handles.SphereHandleCap(
                0,
                points[points.Count - 1],
                Quaternion.identity,
                0.3f,
                EventType.Repaint
            );
        }
        
        // 대기 시간 표시
        if (cargo.waitTimeAtPoint > 0)
        {
            Handles.color = Color.cyan;
            for (int i = 0; i < points.Count; i++)
            {
                Handles.DrawWireDisc(points[i], Vector3.up, 0.2f);
            }
        }
    }
}