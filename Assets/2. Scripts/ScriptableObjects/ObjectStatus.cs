using UnityEngine;

[CreateAssetMenu(fileName ="StatusSO", menuName = "Scriptable Objects/Status")]
public class ObjectStatus : ScriptableObject
{
    //hp
    public int hp = 10;
    //인식 범위
    public float recognizeRange = 10;
    //이동 속도
    public float moveSpeed = 1;
    //데미지
    public int damage = 1;
    //공격 속도
    public float attackSpeed = 1.0f; //애니메이션 재생속도
    //공격 범위
    public float attackRange = 2;
}
