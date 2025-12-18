using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFairy : FairyBase
{
    [SerializeField] private FireFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(0,1,-3);
    private float mSlerpSpeed = 5.0f;

    public void SetUp(FireFairySkillDataSO skillDataSO) 
    {
        //페어리 데이터 스텟 받아오기
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mSeatNumber = skillDataSO.SeatNumber;

        Utils.Log($"{mPlayer.name}");
        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mPlayer.gameObject.transform.position + mPlayer.gameObject.transform.rotation * mOffset;
        transform.position = Vector3.Slerp(transform.position, targetPos, mSlerpSpeed * Time.deltaTime);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyFire");
        //화염 데미지 = 데미지 * 0.2(기존 데미지 20%), 3초동안 0.2초 간격
        float fireDamage = damage * mDamageDuplicater;
        target.TakeDotDamage(fireDamage, mEffectTime, mDamageTick, EDmgElement.Fire);
    }
}
