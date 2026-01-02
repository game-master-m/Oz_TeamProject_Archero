using UnityEngine;

public class AutoTurretStrategy : IPassiveStrategy
{
    private float mDamageMultiplier;
    private float mAttackSpeedMultiplier;
    private float mTimer;

    public AutoTurretStrategy(float damageMultiplier, float attackSpeedMultiplier)
    {
        mDamageMultiplier = damageMultiplier;
        mAttackSpeedMultiplier = attackSpeedMultiplier;
    }

    public void OnEquip(PlayerAttack attack)
    {
        mTimer = 0;
        attack.IsAutoTurret = true;
        attack.Stat.MultipleDamage(mDamageMultiplier);
        attack.Stat.MultipleAttackSpeed(mAttackSpeedMultiplier);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        mTimer += Time.deltaTime;
        if (mTimer >= 1 / attack.Stat.AttackSpeed)
        {
            mTimer = 0;
            attack.MakeProjectile();
        }
    }

    public void OnUnequip(PlayerAttack attack)
    {

    }
}
