using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFairyStrategy : IPassiveStrategy
{
    private LaserFairySkillDataSO mLaserFairyData;
    private LaserFairy mLaserFairyPrefab;

    private LaserFairy mLaserFairy;

    public LaserFairyStrategy(LaserFairySkillDataSO fairySkillDataSO)
    {
        mLaserFairyData = fairySkillDataSO;
        mLaserFairyPrefab = mLaserFairyData.LaserFairyPrefab;

        Managers.Pool.CreatePool(mLaserFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mLaserFairy = Managers.Pool.GetFromPool(mLaserFairyPrefab);
        mLaserFairy.SetUp(mLaserFairyData, attack);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mLaserFairy.StopAllCoroutines();
        mLaserFairy.Detach();
        Managers.Pool.ReturnToPool(mLaserFairy);
    }
}
