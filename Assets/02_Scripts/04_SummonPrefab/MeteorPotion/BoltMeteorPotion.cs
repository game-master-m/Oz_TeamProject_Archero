using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltMeteorPotion : PotionBase
{
    private BoltMeteorPotionSkillDataSO mSkillDataSO;
    private PlayerAttack mPlayer;
    private BoltMeteor mMeteorPrefab;

    private Vector3 mMeteorOffset;

    private BoltMeteor mBoltMeteor;

    //세팅
    public void SetUp(BoltMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mSkillDataSO = skillDataSO;
        mPlayer = attack;
        mMeteorPrefab = skillDataSO.MeteorPrefab;
        mMeteorOffset = skillDataSO.MeteorOffset;
        Managers.Pool.CreatePool(mMeteorPrefab, 1, Managers.Pool.transform);
        SetPosition(attack);
    }

    public override void ApplyPotionEffect()
    {
        //메테오 생성
        mBoltMeteor = Managers.Pool.GetFromPool(mMeteorPrefab);
        //위쪽에 띄우기 > 중력 받아서 떨어짐
        mBoltMeteor.gameObject.transform.position = this.gameObject.transform.position + mMeteorOffset;
        mBoltMeteor.SetUp(mSkillDataSO, mPlayer);
        Managers.Pool.ReturnToPool(this);
    }
}
