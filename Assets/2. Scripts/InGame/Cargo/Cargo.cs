using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using System;

public class Cargo : MonoBehaviour
{
    [SerializeField]
    private List<SerializableVector3> serializedPathPoints = new List<SerializableVector3>();

    [HideInInspector]
    public List<Vector3> pathPoints = new List<Vector3>();

    public float moveSpeed = 2f;
    public float waitTimeAtPoint = 1f;
    public int width = 6;  // N tiles
    public int height = 6; // M tiles
    public bool autoStart = false;

    private int currentPathIndex = 0;
    private bool isMoving = false;
    private Vector3 currentMoveTarget;
    private Coroutine moveCoroutine;

    public event Action OnDestinationReached;

    private void OnValidate()
    {
        UpdatePathPoints();
    }

    private void Awake()
    {
        UpdatePathPoints();
    }

    private void UpdatePathPoints()
    {
        pathPoints.Clear();
        foreach (var point in serializedPathPoints)
        {
            pathPoints.Add(point.ToVector3());
        }
    }

    public void AddPathPoint(Vector3 point)
    {
        serializedPathPoints.Add(new SerializableVector3(point));
        UpdatePathPoints();
    }

    public void RemovePathPoint(int index)
    {
        if (index >= 0 && index < serializedPathPoints.Count)
        {
            serializedPathPoints.RemoveAt(index);
            UpdatePathPoints();
        }
    }

    public void ClearPath()
    {
        serializedPathPoints.Clear();
        UpdatePathPoints();
    }

    private void Start()
    {
        if (autoStart && pathPoints.Count > 0)
        {
            StartMoving();
        }
    }

    public void StartMoving()
    {
        if (pathPoints.Count > 0)
        {
            isMoving = true;
            currentPathIndex = 0;
            currentMoveTarget = pathPoints[0];
            transform.position = pathPoints[0];

            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveAlongPath());
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (currentPathIndex < pathPoints.Count && isMoving)
        {
            currentMoveTarget = pathPoints[currentPathIndex];
            
            while (Vector3.Distance(transform.position, currentMoveTarget) > 0.01f)
            {
                if (!isMoving) yield break;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentMoveTarget,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            // 정확한 위치로 설정
            transform.position = currentMoveTarget;

            // 마지막 지점이 아니라면 대기
            if (currentPathIndex < pathPoints.Count - 1 && waitTimeAtPoint > 0)
            {
                yield return new WaitForSeconds(waitTimeAtPoint);
            }

            currentPathIndex++;

            // 모든 경로를 이동했다면 정지
            if (currentPathIndex >= pathPoints.Count)
            {
                isMoving = false;
                OnDestinationReached?.Invoke();
                yield break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pathPoints.Count < 2) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pathPoints[0], 0.3f);
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);

            if (waitTimeAtPoint > 0 && i < pathPoints.Count - 1)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(pathPoints[i], 0.2f);
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawWireSphere(pathPoints[i + 1], 0.1f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pathPoints[pathPoints.Count - 1], 0.3f);

        if (Application.isPlaying && isMoving && currentPathIndex < pathPoints.Count - 1)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentMoveTarget);
        }
    }
}