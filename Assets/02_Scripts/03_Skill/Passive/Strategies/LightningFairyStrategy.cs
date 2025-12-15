using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningFairyStrategy : IPassiveStrategy
{
    private LightningFairySkillDataSO mLightningFairyData;
    private LightningFairy mLightningFairyPrefab;

    private LightningFairy mLightningFairy;

    public LightningFairyStrategy(LightningFairySkillDataSO fairySkillDataSO)
    {
        mLightningFairyData = fairySkillDataSO;
        mLightningFairyPrefab = mLightningFairyData.LightningFairyPrefab;

        Managers.Pool.CreatePool(mLightningFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mLightningFairy = Managers.Pool.GetFromPool(mLightningFairyPrefab);
        mLightningFairy.SetOwner(attack);
        mLightningFairy.SetUp(mLightningFairyData);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mLightningFairy.StopAllCoroutines();
        mLightningFairy.Detach();
        Managers.Pool.ReturnToPool(mLightningFairy);
    }
}
