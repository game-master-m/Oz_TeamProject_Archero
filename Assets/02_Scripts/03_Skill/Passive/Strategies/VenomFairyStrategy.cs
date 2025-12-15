using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomFairyStrategy : IPassiveStrategy
{
    private VenomFairySkillDataSO mVenomFairyData;
    private VenomFairy mVenomFairyPrefab;

    private VenomFairy mVenomFairy;

    public VenomFairyStrategy(VenomFairySkillDataSO fairySkillDataSO)
    {
        mVenomFairyData = fairySkillDataSO;
        mVenomFairyPrefab = mVenomFairyData.VenomFairyPrefab;

        Managers.Pool.CreatePool(mVenomFairyPrefab, 1, Managers.Pool.transform);
    }

    public void OnEquip(PlayerAttack attack)
    {
        mVenomFairy = Managers.Pool.GetFromPool(mVenomFairyPrefab);
        mVenomFairy.SetOwner(attack);
        mVenomFairy.SetUp(mVenomFairyData);
    }

    public void OnUpdate(PlayerAttack attack)
    {

    }

    public void OnUnequip(PlayerAttack attack)
    {
        mVenomFairy.StopAllCoroutines();
        mVenomFairy.Detach();
        Managers.Pool.ReturnToPool(mVenomFairy);
    }
}
