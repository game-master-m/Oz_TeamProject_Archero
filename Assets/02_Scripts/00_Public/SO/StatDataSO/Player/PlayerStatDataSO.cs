using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Archero/Stat/PlayerStatData")]
public class PlayerStatDataSO : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float rotateSpeed = 8.0f;

    [Header("Growth Stats")]
    [SerializeField] private float hpPerLevel = 10f;
    [SerializeField] private float damagePerLevel = 2f;

    public float MaxHp => maxHp;
    public float MoveSpeed => moveSpeed;
    public float AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float RotateSpeed => rotateSpeed;
    public float HPPerLevel => hpPerLevel;
    public float DamagePerLevel => damagePerLevel;
}
