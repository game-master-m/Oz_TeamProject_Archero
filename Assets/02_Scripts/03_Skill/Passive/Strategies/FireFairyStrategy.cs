using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFairyStrategy : IPassiveStrategy
{
    private FireFairySkillDataSO mFireFairyData;
    private FireFairy mFireFairyPrefab;

    private FireFairy mFireFairy;

    public FireFairyStrategy(FireFairySkillDataSO fairySkillDataSO)
    {
        mFireFairyData = fairySkillDataSO;
        mFireFairyPrefab = mFireFairyData.FireFairyPrefab;
  
        Managers.Pool.CreatePool(mFireFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack) 
    {
        mFireFairy = Managers.Pool.GetFromPool(mFireFairyPrefab);
        mFireFairy.SetUp(mFireFairyData, attack);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        
    }

    public void OnUnequip(PlayerAttack attack)
    {
        if (mFireFairy == null) return;
        mFireFairy.StopAllCoroutines();
        mFireFairy.Detach();
        Managers.Pool.ReturnToPool(mFireFairy);
    }
}
