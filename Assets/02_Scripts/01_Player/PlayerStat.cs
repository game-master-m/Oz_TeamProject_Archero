using System.Collections;
using UnityEngine;

public class PlayerStat : LivingEntity
{
    [Header("Data Source")]
    [SerializeField] private PlayerStatDataSO mStat;

    [Header("이벤트 발행")]
    [SerializeField] private VoidEventChannelSO mOnPlayerDie;       //StageManger.cs 가 구독
    //새로추가함
    public PlayerStatDataSO StatDataSO => mStat;
    public float AttackDamage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public float RotateSpeed { get; private set; }
    public float AttackRange { get; private set; }


    private float mDieDelay = 0.5f;

    protected override void OnEnable()
    {
        base.OnEnable();

        InitStats();
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    // 초기화 메서드 (레벨업이나 부활 시에도 사용 가능)
    public void InitStats()
    {
        //Hp초기화
        base.Init(mStat.MaxHp);

        AttackDamage = mStat.AttackDamage;
        MoveSpeed = mStat.MoveSpeed;
        AttackSpeed = mStat.AttackSpeed;
        RotateSpeed = mStat.RotateSpeed;
        AttackRange = mStat.AttackRange;
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
    public void AddHP(float amount)
    {
        mCurrentHP += amount;
        if (mCurrentHP > MaxHP) mCurrentHP = MaxHP;
        UpdateHPRequest(mCurrentHP / MaxHP);
    }
    public void MultipleHP(float amount)
    {
        mCurrentHP *= amount;
        if (mCurrentHP > MaxHP) mCurrentHP = MaxHP;
        UpdateHPRequest(mCurrentHP / MaxHP);
    }
    public void AddMaxHP(float amount)
    {
        mMaxHP += amount;
        mCurrentHP += amount;
        UpdateHPRequest(mCurrentHP / MaxHP);
    }
    public void MultipleMaxHP(float amount)
    {
        mMaxHP *= amount;
        mCurrentHP *= amount;
        UpdateHPRequest(mCurrentHP / MaxHP);
    }
    public void MultipleMaxHPAndRecoverAll(float amount)
    {
        MultipleMaxHP(amount);
        mCurrentHP = mMaxHP;
        UpdateHPRequest(mCurrentHP / MaxHP);
    }
    public void MultipleAttackRange(float amount)
    {
        AttackRange *= amount;
    }
    #endregion

    public override void Die()
    {
        base.Die();
        Utils.Log("플레이어 다이~!");

        //Player Die 시 호출
        StartCoroutine(DelayAndDieBroadCastCO());
    }

    private IEnumerator DelayAndDieBroadCastCO()
    {
        yield return new WaitForSeconds(mDieDelay);
        mOnPlayerDie.Raised();
    }
}