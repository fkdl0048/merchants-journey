using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StatusSO", menuName = "Scriptable Objects/Status")]
public class ObjectStatus : ScriptableObject
{
    //ü��
    public int hp = 10;
    //�ν� ����
    public float recognizeRange = 10;
    //�̵� �ӵ�
    public float moveSpeed = 1;
    //���ݷ�
    public float damage = 1;
    //���� �ӵ�
    public float attackSpeed = 1.0f; //�ִϸ��̼� ��� �ӵ�
    //���� ��Ÿ�
    public float attackRange = 2;
}
