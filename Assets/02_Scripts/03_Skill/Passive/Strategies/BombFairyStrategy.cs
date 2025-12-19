using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFairyStrategy : IPassiveStrategy
{
    private BombFairySkillDataSO mBombFairyData;
    private BombFairy mBombFairyPrefab;

    private BombFairy mBombFairy;

    public BombFairyStrategy(BombFairySkillDataSO fairySkillDataSO)
    {
        mBombFairyData = fairySkillDataSO;
        mBombFairyPrefab = mBombFairyData.BombFairyPrefab;

        Managers.Pool.CreatePool(mBombFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mBombFairy = Managers.Pool.GetFromPool(mBombFairyPrefab);
        mBombFairy.SetUp(mBombFairyData, attack);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mBombFairy.StopAllCoroutines();
        mBombFairy.Detach();
        Managers.Pool.ReturnToPool(mBombFairy);
    }
}
