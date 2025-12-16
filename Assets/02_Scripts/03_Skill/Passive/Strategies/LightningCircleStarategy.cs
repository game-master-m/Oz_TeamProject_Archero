using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCircleStarategy : IPassiveStrategy
{
    private LightningCircleSkillDataSO mSkillDataSO;
    private LightningCircle mLightningCirclePrefab;

    private LightningCircle mLightningCircle;

    public LightningCircleStarategy(LightningCircleSkillDataSO skillDataSO)
    {
        mLightningCirclePrefab = skillDataSO.LightningCirclePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mLightningCirclePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mLightningCircle = Managers.Pool.GetFromPool(mLightningCirclePrefab);

        mLightningCircle.SetOwner(attack);
        mLightningCircle.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mLightningCircle.Detach();
        Managers.Pool.ReturnToPool(mLightningCircle);
    }
}
