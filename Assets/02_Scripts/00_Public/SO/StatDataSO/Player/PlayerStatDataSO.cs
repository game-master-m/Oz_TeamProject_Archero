using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Archero/Stat/PlayerStatData")]
public class PlayerStatDataSO : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private float mMaxHp = 100f;
    [SerializeField] private float mMoveSpeed = 5f;
    [SerializeField] private float mAttackDamage = 10f;
    [SerializeField] private float mAttackRange = 30f;
    [SerializeField] private float mAttackSpeed = 1f;
    [SerializeField] private float mRotateSpeed = 8.0f;

    [Header("Growth Stats")]
    [SerializeField] private float mHpPerLevel = 10f;
    [SerializeField] private float mDamagePerLevel = 2f;

    public float MaxHp => mMaxHp;
    public float MoveSpeed => mMoveSpeed;
    public float AttackDamage => mAttackDamage;
    public float AttackRange => mAttackRange;
    public float AttackSpeed => mAttackSpeed;
    public float RotateSpeed => mRotateSpeed;
    public float HPPerLevel => mHpPerLevel;
    public float DamagePerLevel => mDamagePerLevel;
}
