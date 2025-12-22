using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyBoostStarategy : IPassiveStrategy
{
    public FairyBoostStarategy(FairyBoostSkillDataSO fairySkillDataSO)
    {

    }

    public void OnEquip(PlayerAttack attack)
    {
        FairyReinforceStatic.FairyAttackDamageDuplicater = 1.4f;
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        FairyReinforceStatic.FairyAttackDamageDuplicater = 1f;
    }
}
