using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StatusSO", menuName = "Scriptable Objects/Status")]
public class ObjectStatus : ScriptableObject
{
    //체력
    public int hp = 10;
    //인식 범위
    public float recognizeRange = 10;
    //이동 속도
    public float moveSpeed = 1;
    //공격력
    public float damage = 1;
    //공격 속도
    public float attackSpeed = 1.0f; //애니메이션 재생 속도
    //공격 사거리
    public float attackRange = 2;
}
