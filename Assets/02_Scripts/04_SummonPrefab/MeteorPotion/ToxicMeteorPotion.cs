using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMeteorPotion : PotionBase
{
    private ToxicMeteorPotionSkillDataSO mSkillDataSO;
    private PlayerAttack mPlayer;
    private ToxicMeteor mMeteorPrefab;

    private Vector3 mMeteorOffset;

    private ToxicMeteor mToxicMeteor;

    //세팅
    public void SetUp(ToxicMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
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
        mToxicMeteor = Managers.Pool.GetFromPool(mMeteorPrefab);
        //위쪽에 띄우기 > 중력 받아서 떨어짐
        mToxicMeteor.gameObject.transform.position = this.gameObject.transform.position + mMeteorOffset;
        mToxicMeteor.SetUp(mSkillDataSO, mPlayer);
        Managers.Pool.ReturnToPool(this);
    }
}
