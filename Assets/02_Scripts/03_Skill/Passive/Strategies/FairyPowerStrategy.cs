using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyPowerStrategy : IPassiveStrategy
{
    public FairyPowerStrategy(FairyPowerSkillDataSO fairySkillDataSO)
    {

    }

    public void OnEquip(PlayerAttack attack)
    {
        FairyReinforceStatic.FairyAttackDuplicater = 1.5f;
        FairyReinforceStatic.FairyAttackSpeedDuplicater = 1.5f;
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        FairyReinforceStatic.FairyAttackDuplicater = 1f;
        FairyReinforceStatic.FairyAttackSpeedDuplicater = 1f;
    }
}
