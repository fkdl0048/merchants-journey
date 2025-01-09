using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using System;
using Scripts.Manager;

public class Cargo : MonoBehaviour
{
    [SerializeField]
    private List<SerializableVector3> serializedPathPoints = new List<SerializableVector3>();

    [HideInInspector]
    public List<Vector3> pathPoints = new List<Vector3>();

    public float maxHP = 10;
    public float hp;
    public float moveSpeed = 0.3f;
    public float OriginMoveSpeed = 0.3f;
    public float waitTimeAtPoint = 1f;
    public int width = 6;  // N tiles 
    public int height = 6; // M tiles
    public bool autoStart = false;

    private int currentPathIndex = 0;
    private bool isMoving = false;
    private Vector3 currentMoveTarget;
    private Coroutine moveCoroutine;

    public event Action OnDestinationReached;
    public event Action<object[]> OnStartMoving;
    public event Action<object[]> OnStopMoving;

    private void OnValidate()
    {
        UpdatePathPoints();
    }

    private void Awake()
    {
        UpdatePathPoints();

        //적군이 스폰이 될 경우, 마차의 이동속도가 감소.
        OriginMoveSpeed = moveSpeed;
        hp = maxHP;
    }
    //길 따라가는 스크립트 관련//
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
    public void StartMoving()
    {
        if (isMoving == true)
            return;
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
        if (isMoving == false)
            return;
        isMoving = false;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    public void RestartMoving()
    {
        if (isMoving == true)
            return;
        isMoving = true;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveAlongPath());
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

    //디버깅
    private void OnDrawGizmos()
    {
        //
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