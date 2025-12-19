using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeMeteorPotion : PotionBase
{
    private BlazeMeteorPotionSkillDataSO mSkillDataSO;
    private PlayerAttack mPlayer;
    private BlazeMeteor mMeteorPrefab;

    private Vector3 mMeteorOffset;

    private BlazeMeteor mBlazeMeteor;

    //세팅
    public void SetUp(BlazeMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack) 
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
        mBlazeMeteor = Managers.Pool.GetFromPool(mMeteorPrefab);
        //위쪽에 띄우기 > 중력 받아서 떨어짐
        Vector3 targetPos = FindCloseEnemy().position;
        if (targetPos == null) 
        {
            targetPos = this.gameObject.transform.position;
        }
        mBlazeMeteor.gameObject.transform.position = targetPos + mMeteorOffset;
        mBlazeMeteor.SetUp(mSkillDataSO, mPlayer);
        Managers.Pool.ReturnToPool(this);
    }
}
