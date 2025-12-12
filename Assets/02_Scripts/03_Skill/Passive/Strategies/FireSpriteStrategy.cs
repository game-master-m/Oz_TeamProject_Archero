using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpriteStrategy : IPassiveStrategy
{
    private FireFairySkillDataSO mFireFairyData;
    private FireFairy mFireFairyPrefab;

    private FireFairy mFireFairy;

    public FireSpriteStrategy(FireSpriteSkillDataSO fairySkillDataSO)
    {

    }

    public void OnEquip(PlayerAttack attack) 
    {
        mFireFairy = Managers.Pool.GetFromPool(mFireFairyPrefab);
        mFireFairy.SetOwner(attack);
        mFireFairy.SetUp(mFireFairyData);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        
    }

    public void OnUnequip(PlayerAttack attack)
    {
        mFireFairy.StopAllCoroutines();
        mFireFairy.Detach();
        Managers.Pool.ReturnToPool(mFireFairy);
    }
}
