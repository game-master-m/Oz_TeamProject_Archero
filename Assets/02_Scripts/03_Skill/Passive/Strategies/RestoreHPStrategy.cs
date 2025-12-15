using UnityEngine;

public class RestoreHPStrategy : IPassiveStrategy, ISkillStackable<IPassiveStrategy>
{
    private float mRandomHpMultiplier;
    public void OnEquip(PlayerAttack attack)
    {
        mRandomHpMultiplier = Random.Range(1.1f, 1.4f);
        attack.Stat.MultipleHP(mRandomHpMultiplier);
    }

    public void OnUnequip(PlayerAttack attack) { }

    public void OnUpdate(PlayerAttack attack) { }

    public bool TryStack(IPassiveStrategy strategy)
    {
        if (strategy is RestoreHPStrategy)
        {
            return true;
        }
        return false;
    }
}
