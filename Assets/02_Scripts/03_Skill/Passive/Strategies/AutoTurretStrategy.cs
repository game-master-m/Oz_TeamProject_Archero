using UnityEngine;

public class AutoTurretStrategy : IPassiveStrategy
{
    private float mDamageMultiplier;
    private float mTimer;

    public AutoTurretStrategy(float damageMultiplier)
    {
        mDamageMultiplier = damageMultiplier;
    }

    public void OnEquip(PlayerAttack attack)
    {
        mTimer = 0;
        attack.IsAutoTurret = true;
        attack.AttackSpeed *= mDamageMultiplier;
    }

    public void OnUpdate(PlayerAttack attack)
    {
        mTimer += Time.deltaTime;
        if (mTimer >= 1 / attack.AttackSpeed)
        {
            mTimer = 0;
            attack.MakeProjectile();
        }
    }

    public void OnUnequip(PlayerAttack attack)
    {

    }
}
