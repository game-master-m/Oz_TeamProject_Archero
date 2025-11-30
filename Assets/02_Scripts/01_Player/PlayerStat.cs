using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerStat : LivingEntity
{
    [Header("Data Source")]
    [SerializeField] private PlayerStatDataSO stat;

    public float AttackDamage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public float RotateSpeed { get; private set; }

    private void Awake()
    {
        // 나중에 Save 데이터나 SO에서 불러오는 로직으로 교체
        InitStats();
    }

    // 초기화 메서드 (레벨업이나 부활 시에도 사용 가능)
    public void InitStats()
    {
        //Hp초기화
        base.Init(stat.MaxHp);

        AttackDamage = stat.AttackDamage;
        MoveSpeed = stat.MoveSpeed;
        AttackSpeed = stat.AttackSpeed;
        RotateSpeed = stat.RotateSpeed;
        //
    }

    //스탯변경 로직 필요(레벨 업, 아이템 등)
    #region 스탯변경 메서드
    public void AddDamage(float amount)
    {
        AttackDamage += amount;
    }
    public void MultipleDamage(float amount)
    {
        AttackDamage *= amount;
    }

    public void AddMoveSpeed(float amount)
    {
        MoveSpeed += amount;
    }
    public void MultipleMoveSpeed(float amount)
    {
        MoveSpeed *= amount;
    }
    public void AddAttackSpeed(float amount)
    {
        AttackSpeed += amount;
    }
    public void MultipleAttackSpeed(float amount)
    {
        AttackSpeed *= amount;
    }
    #endregion

    public override void Die()
    {
        base.Die();

        //Player Die 시 호출
    }
}