using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretStrategy : IPassiveStrategy
{
    private float mAttackSpeedMultiplier;
    private float mTimer;

    public AutoTurretStrategy(float attackSpeedMultiplier)
    {
        mAttackSpeedMultiplier = attackSpeedMultiplier;
    }

    public void OnEquip(PlayerAttack attack)
    {
        mTimer = 0;
        attack.IsAutoTurret = true;
        attack.AttackSpeed *= mAttackSpeedMultiplier;
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
