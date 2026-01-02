using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFairyStrategy : IPassiveStrategy
{
    private SuperFairySkillDataSO mSuperFairyData;
    private SuperFairy mSuperFairyPrefab;

    private SuperFairy mSuperFairy;

    public SuperFairyStrategy(SuperFairySkillDataSO fairySkillDataSO)
    {
        mSuperFairyData = fairySkillDataSO;
        mSuperFairyPrefab = mSuperFairyData.SuperFairyPrefab;

        Managers.Pool.CreatePool(mSuperFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mSuperFairy = Managers.Pool.GetFromPool(mSuperFairyPrefab);
        mSuperFairy.SetUp(mSuperFairyData, attack);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        if (mSuperFairy == null) return;
        mSuperFairy.StopAllCoroutines();
        mSuperFairy.Detach();
        Managers.Pool.ReturnToPool(mSuperFairy);
    }
}
