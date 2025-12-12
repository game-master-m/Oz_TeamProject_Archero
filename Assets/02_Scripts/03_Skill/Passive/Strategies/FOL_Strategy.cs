using UnityEngine;

public class FOL_Strategy : IPassiveStrategy, ISkillStackable<IPassiveStrategy>
{
    private float mMaxHPMultiplier;

    public FOL_Strategy(float maxHPMultiplier)
    {
        mMaxHPMultiplier = maxHPMultiplier;
    }

    //OnEquip은 스킬을 선택했을 때, 항상 호출 됨 (추후 문제 생길 시 보완)
    public void OnEquip(PlayerAttack attack)
    {
        attack.Stat.MultipleMaxHPAndRecoverAll(mMaxHPMultiplier);
    }

    public void OnUnequip(PlayerAttack attack)
    {

    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    //플레이어 스탯 관련 Passive스킬은 PlayerAttack.cs의 AddSkill에서 항상 OnEquip()메서드가 실행되기 때문에,
    //TryStack은 true, false 리턴만
    public bool TryStack(IPassiveStrategy strategy)
    {
        if (strategy is FOL_Strategy)
        {
            return true;
        }
        return false;
    }
}
