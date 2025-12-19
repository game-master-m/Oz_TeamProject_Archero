using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCircleStrategy : IPassiveStrategy
{
    private LaserCircleSkillDataSO mSkillDataSO;
    private LaserCircle mLaserCirclePrefab;

    private LaserCircle mLaserCircle;

    public LaserCircleStrategy(LaserCircleSkillDataSO skillDataSO)
    {
        mLaserCirclePrefab = skillDataSO.LaserCirclePrefab;
        mSkillDataSO = skillDataSO;

        Managers.Pool.CreatePool(mLaserCirclePrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mLaserCircle = Managers.Pool.GetFromPool(mLaserCirclePrefab);

        mLaserCircle.SetOwner(attack);
        mLaserCircle.SetUp(mSkillDataSO);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mLaserCircle.Detach();
        Managers.Pool.ReturnToPool(mLaserCircle);
    }
}
