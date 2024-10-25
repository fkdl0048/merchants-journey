using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [SerializeField] private GameObject enemyObj;       // 테스트 스크립트
    [SerializeField] private Transform[] spawnPoints;   // 스폰 포인트  Transform 정보
    [SerializeField] private float spawnTime = 2.5f;    // 스폰 간격
    [SerializeField] private int spawnNumber = 3;       // 한번에 스폰되는 몬스터 수

    private void Awake()
    {
        if (spawnPoints.Length > 0)
            Play();
    }

    private void Play()
    {
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(spawnTime);

        //가장 가까운 스폰스팟 찾기
        Transform nearT = default;
        float dist = float.MaxValue;
        foreach (Transform t in spawnPoints)
        {
            float d = Vector2.Distance(t.position, target.position);
            if (d < dist)
            {
                dist = d;
                nearT = t;
            }
        }

        //몬스터 스폰
        for (int i = 0; i < spawnNumber; i++)
        {
            var obj = Instantiate(enemyObj);
            obj.transform.parent = transform;
            obj.transform.position = nearT.position;

            obj.GetComponent<EnemyMovement>().target = target;
        }

        yield return wait;

        StartCoroutine(nameof(Spawn));
    }
}
