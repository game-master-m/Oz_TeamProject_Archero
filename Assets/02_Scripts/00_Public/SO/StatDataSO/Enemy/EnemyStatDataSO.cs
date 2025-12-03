using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStatDataSO", menuName = "Archero/Stat/EnemyStatDataSO")]
public class EnemyStatDataSO : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private float mMaxHP = 100f;
    [SerializeField] private float mMoveSpeed = 5f;
    [SerializeField] private float mAttackDamage = 10f;
    [SerializeField] private float mAttackRange = 10f;
    [SerializeField] private float mAttackSpeed = 1f;
    [SerializeField] private float mRotateSpeed = 8.0f;

    public float MaxHP => mMaxHP;
    public float MoveSpeed => mMoveSpeed;
    public float AttackDamage => mAttackDamage;
    public float AttackRange => mAttackRange;
    public float AttackSpeed => mAttackSpeed;
    public float RotateSpeed => mRotateSpeed;
}
