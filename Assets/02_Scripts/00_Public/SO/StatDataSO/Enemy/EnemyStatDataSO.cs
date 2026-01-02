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
    [SerializeField] private float mHitTiming = 3.0f;
    [SerializeField] private float mRotateSpeed = 8.0f;
    [SerializeField] private float mDetectRange = 20.0f;
    [SerializeField] private int mMaxExpDropCount = 1;

    [Header("프리팹 좌표 오프셋(Forward 와 Z축 일치)")]
    [SerializeField] private Vector3 mRotateOffset = new Vector3(0.0f, 90.0f, 0.0f);

    public float MaxHP => mMaxHP;
    public float MoveSpeed => mMoveSpeed;
    public float AttackDamage => mAttackDamage;
    public float AttackRange => mAttackRange;
    public float AttackSpeed => mHitTiming;
    public float RotateSpeed => mRotateSpeed;
    public float DetectRange => mDetectRange;
    public int MaxExpDropCount => mMaxExpDropCount;
    public Vector3 RotateOffset => mRotateOffset;
}
