using UnityEngine;

public class GiantStrengthStrategy : IPassiveStrategy, ISkillStackable<IPassiveStrategy>
{
    private float mAttackDamageMultiplier;
    private float mMaxHPMultiplier;
    private float mMoveSpeedMultiplier;

    public GiantStrengthStrategy(float attackDamageMultiplier, float maxHPMultiplier, float moveSpeedMultiplier)
    {
        mAttackDamageMultiplier = attackDamageMultiplier;
        mMaxHPMultiplier = maxHPMultiplier;
        mMoveSpeedMultiplier = moveSpeedMultiplier;
    }

    public void OnEquip(PlayerAttack attack)
    {
        attack.Stat.MultipleDamage(mAttackDamageMultiplier);
        attack.Stat.MultipleMaxHP(mMaxHPMultiplier);
        attack.Stat.MultipleMoveSpeed(mMoveSpeedMultiplier);
    }

    public void OnUnequip(PlayerAttack attack) { }

    public void OnUpdate(PlayerAttack attack) { }

    //플레이어 스탯 관련 Passive스킬은 PlayerAttack.cs의 AddSkill에서 항상 OnEquip()메서드가 실행되기 때문에,
    //TryStack은 true, false 리턴만
    public bool TryStack(IPassiveStrategy strategy)
    {
        if (strategy is GiantStrengthStrategy)
        {
            return true;
        }
        return false;
    }
}
