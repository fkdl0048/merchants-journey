using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;
using UnityEditor;

public class GridTileEditor : EditorWindow
{
    private GameObject tilePrefab;
    private GameObject obstaclePrefab;
    private float tileSize = 1f;
    private Transform parentTransform;
    private bool shouldSnapToGrid = true;
    private bool showPreview = true;
    private Material previewMaterial;
    private Material obstaclePreviewMaterial;
    private bool useColliderBounds = true;
    private TileType currentTileType = TileType.Normal;
    
    private bool isDragging = false;
    private Vector3 lastDragPosition;
    private HashSet<Vector3> placedPositions = new HashSet<Vector3>();

    [MenuItem("Tools/Grid Tile Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridTileEditor>("Grid Tile Editor");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        
        previewMaterial = new Material(Shader.Find("Standard"));
        previewMaterial.color = new Color(0, 1, 0, 0.5f);

        obstaclePreviewMaterial = new Material(Shader.Find("Standard"));
        obstaclePreviewMaterial.color = new Color(1, 0, 0, 0.5f);
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        
        if (previewMaterial != null)
            DestroyImmediate(previewMaterial);
        if (obstaclePreviewMaterial != null)
            DestroyImmediate(obstaclePreviewMaterial);
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Tile Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        GUILayout.Label("Basic Settings", EditorStyles.boldLabel);
        
        GameObject oldTilePrefab = tilePrefab;
        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);
        if (tilePrefab != oldTilePrefab && tilePrefab != null)
        {
            if (tilePrefab.GetComponent<Tile>() == null)
            {
                EditorGUILayout.HelpBox("Selected prefab must have a Tile component!", MessageType.Error);
                tilePrefab = oldTilePrefab;
            }
        }

        GameObject oldObstaclePrefab = obstaclePrefab;
        obstaclePrefab = (GameObject)EditorGUILayout.ObjectField("Obstacle Prefab", obstaclePrefab, typeof(GameObject), false);
        if (obstaclePrefab != oldObstaclePrefab && obstaclePrefab != null)
        {
            if (obstaclePrefab.GetComponent<Tile>() == null)
            {
                EditorGUILayout.HelpBox("Selected prefab must have a Tile component!", MessageType.Error);
                obstaclePrefab = oldObstaclePrefab;
            }
        }

        parentTransform = (Transform)EditorGUILayout.ObjectField("Parent Transform (Placement Area)", parentTransform, typeof(Transform), true);
        
        float newTileSize = EditorGUILayout.FloatField("Tile Size", tileSize);
        if (newTileSize != tileSize)
        {
            tileSize = Mathf.Max(1f, Mathf.Round(newTileSize));
        }
        
        EditorGUILayout.Space();
        GUILayout.Label("Placement Settings", EditorStyles.boldLabel);
        
        currentTileType = (TileType)EditorGUILayout.EnumPopup("Current Tile Type", currentTileType);
        shouldSnapToGrid = EditorGUILayout.Toggle("Snap to Grid", shouldSnapToGrid);
        showPreview = EditorGUILayout.Toggle("Show Placement Preview", showPreview);
        useColliderBounds = EditorGUILayout.Toggle("Use Collider Bounds", useColliderBounds);

        if (!useColliderBounds)
        {
            EditorGUILayout.HelpBox("Using Mesh Renderer bounds. Make sure the placement area has a Mesh Renderer.", MessageType.Info);
        }

        EditorGUILayout.Space();
        
        if (GUILayout.Button("Fill Area with Grid"))
        {
            FillAreaWithGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Left Click/Drag: Place tiles/obstacles\n" +
            "Hold Ctrl + Left Click/Drag: Remove tiles/obstacles\n" +
            "Select tile type above to place different types\n" +
            "Make sure the placement area has either a Collider or Mesh Renderer!", 
            MessageType.Info);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!ValidateSetup()) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;
            Vector3 snappedPosition = GetSnappedPosition(hitPoint);
            
            if (showPreview)
            {
                Material previewMat = currentTileType == TileType.Obstacle ? obstaclePreviewMaterial : previewMaterial;
                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                Handles.DrawWireCube(snappedPosition, Vector3.one * tileSize);
            }
            
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        isDragging = true;
                        lastDragPosition = snappedPosition;
                        placedPositions.Clear();
                        ProcessTilePlacement(snappedPosition, e.control);
                        e.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragging)
                    {
                        if (snappedPosition != lastDragPosition)
                        {
                            ProcessTilePlacement(snappedPosition, e.control);
                            lastDragPosition = snappedPosition;
                        }
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        isDragging = false;
                        placedPositions.Clear();
                        e.Use();
                    }
                    break;
            }

            sceneView.Repaint();
        }
    }

    private Vector3 GetSnappedPosition(Vector3 position)
    {
        float halfTileSize = tileSize * 0.5f;

        // Adjust to parent origin and snap to grid
        float relativeX = Mathf.Round((position.x - parentTransform.position.x) / tileSize) * tileSize;
        float relativeZ = Mathf.Round((position.z - parentTransform.position.z) / tileSize) * tileSize;

        return new Vector3(
            parentTransform.position.x + relativeX,
            parentTransform.position.y,
            parentTransform.position.z + relativeZ
        );
    }

    private bool ValidateSetup()
    {
        if (tilePrefab == null || parentTransform == null)
        {
            return false;
        }

        if (tilePrefab.GetComponent<Tile>() == null)
        {
            Debug.LogError("Tile prefab must have a Tile component!");
            return false;
        }

        if (currentTileType == TileType.Obstacle && (obstaclePrefab == null || obstaclePrefab.GetComponent<Tile>() == null))
        {
            Debug.LogError("Obstacle prefab must have a Tile component!");
            return false;
        }

        if (!useColliderBounds && parentTransform.GetComponent<MeshRenderer>() == null)
        {
            return false;
        }

        if (useColliderBounds && parentTransform.GetComponent<Collider>() == null)
        {
            return false;
        }

        return true;
    }

    private void ProcessTilePlacement(Vector3 position, bool isRemoving)
    {
        if (!placedPositions.Add(position)) return;

        if (IsPositionInArea(position))
        {
            if (isRemoving)
            {
                RemoveTileAtPosition(position);
            }
            else
            {
                GameObject gridParent = GetOrCreateGridParent();
                CreateTile(position, gridParent.transform);
            }
        }
    }

    private void CreateTile(Vector3 position, Transform parent)
    {
        if (IsTileExistAtPosition(position))
        {
            UpdateExistingTile(position);
            return;
        }

        GameObject prefabToUse = currentTileType == TileType.Obstacle ? obstaclePrefab : tilePrefab;
        if (prefabToUse == null) return;

        GameObject tileObject = PrefabUtility.InstantiatePrefab(prefabToUse) as GameObject;
        tileObject.transform.parent = parent;
        tileObject.transform.position = position;
        tileObject.transform.rotation = parentTransform.rotation;
        
        Tile tile = tileObject.GetComponent<Tile>();
        if (tile != null)
        {
            tile.tileType = currentTileType;
            tile.isWalkable = currentTileType != TileType.Obstacle && currentTileType != TileType.Unwalkable;
        }
        
        tileObject.name = $"Tile_{currentTileType}_{position.x}_{position.z}";
        Undo.RegisterCreatedObjectUndo(tileObject, "Create Tile");

        if (currentTileType == TileType.Obstacle)
        {
            MakeAdjacentTilesUnwalkable(position);
        }
    }

    private void UpdateExistingTile(Vector3 position)
    {
        Transform gridTransform = parentTransform.Find("Grid");
        if (gridTransform != null)
        {
            foreach (Transform child in gridTransform)
            {
                if (Vector3.Distance(child.position, position) < 0.1f)
                {
                    Tile tile = child.GetComponent<Tile>();
                    if (tile != null)
                    {
                        Undo.RecordObject(tile, "Update Tile Type");
                        tile.tileType = currentTileType;
                        tile.isWalkable = currentTileType != TileType.Obstacle && currentTileType != TileType.Unwalkable;
                    }
                    break;
                }
            }
        }
    }

    private void MakeAdjacentTilesUnwalkable(Vector3 obstaclePosition)
    {
        Transform gridTransform = parentTransform.Find("Grid");
        if (gridTransform != null)
        {
            foreach (Transform child in gridTransform)
            {
                if (Vector3.Distance(child.position, obstaclePosition) < 0.1f)
                {
                    Tile tile = child.GetComponent<Tile>();
                    if (tile != null)
                    {
                        Undo.RecordObject(tile, "Update Tile Walkable Status");
                        tile.isWalkable = false;
                        tile.tileType = TileType.Unwalkable;
                    }
                }
            }
        }
    }

    private bool IsTileExistAtPosition(Vector3 position)
    {
        Transform gridTransform = parentTransform.Find("Grid");
        if (gridTransform != null)
        {
            foreach (Transform child in gridTransform)
            {
                if (Vector3.Distance(child.position, position) < 0.1f)
                    return true;
            }
        }
        return false;
    }

    private void RemoveTileAtPosition(Vector3 position)
    {
        Transform gridTransform = parentTransform.Find("Grid");
        if (gridTransform != null)
        {
            foreach (Transform child in gridTransform)
            {
                if (Vector3.Distance(child.position, position) < 0.1f)
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    break;
                }
            }
        }
    }

    private GameObject GetOrCreateGridParent()
    {
        Transform gridTransform = parentTransform.Find("Grid");
        if (gridTransform == null)
        {
            GameObject gridParent = new GameObject("Grid");
            gridParent.transform.parent = parentTransform;
            return gridParent;
        }
        return gridTransform.gameObject;
    }

    private bool IsPositionInArea(Vector3 position)
    {
        if (useColliderBounds)
        {
            Collider collider = parentTransform.GetComponent<Collider>();
            return collider.bounds.Contains(position);
        }
        else
        {
            MeshRenderer renderer = parentTransform.GetComponent<MeshRenderer>();
            return renderer.bounds.Contains(position);
        }
    }

    private Bounds GetAreaBounds()
    {
        if (useColliderBounds)
        {
            return parentTransform.GetComponent<Collider>().bounds;
        }
        return parentTransform.GetComponent<MeshRenderer>().bounds;
    }

    private void FillAreaWithGrid()
    {
        if (!ValidateSetup()) return;

        Bounds bounds = GetAreaBounds();
        GameObject gridParent = GetOrCreateGridParent();

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        for (float x = min.x; x <= max.x; x += tileSize)
        {
            for (float z = min.z; z <= max.z; z += tileSize)
            {
                Vector3 position = new Vector3(
                    Mathf.Round(x / tileSize) * tileSize,
                    parentTransform.position.y,
                    Mathf.Round(z / tileSize) * tileSize
                );

                if (IsPositionInArea(position))
                {
                    CreateTile(position, gridParent.transform);
                }
            }
        }
    }

    private void ClearGrid()
    {
        if (parentTransform != null)
        {
            GameObject gridObject = parentTransform.Find("Grid")?.gameObject;
            if (gridObject != null)
            {
                Undo.DestroyObjectImmediate(gridObject);
            }
        }
    }
}