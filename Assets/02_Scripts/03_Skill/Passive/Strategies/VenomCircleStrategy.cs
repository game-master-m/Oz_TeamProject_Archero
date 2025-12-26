using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomCircleStrategy : IPassiveStrategy
{
    private VenomCircleSkillDataSO mSkillDataSO;
    private VenomCircle mVenomCirclePrefab;

    private VenomCircle mVenomCircle;

    public VenomCircleStrategy(VenomCircleSkillDataSO skillDataSO)
    {
        mVenomCirclePrefab = skillDataSO.VenomCirclePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mVenomCirclePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mVenomCircle = Managers.Pool.GetFromPool(mVenomCirclePrefab);

        mVenomCircle.SetOwner(attack);
        mVenomCircle.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        if (mVenomCircle == null) return;
        mVenomCircle.Detach();
        Managers.Pool.ReturnToPool(mVenomCircle);
    }
}
