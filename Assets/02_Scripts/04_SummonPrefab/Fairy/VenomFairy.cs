using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomFairy : FairyBase
{
    [SerializeField] private VenomFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(-2, 1, -3);
    private float mSlerpSpeed = 5.0f;

    public void SetUp(VenomFairySkillDataSO skillDataSO, PlayerAttack attack)
    {
        SetOwner(attack);

        //페어리 데이터 스텟 받아오기
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mSeatNumber = skillDataSO.SeatNumber;

        Utils.Log($"{mPlayer.name}");
        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up,mSeatAngle * mSeatNumber);
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mPlayer.gameObject.transform.position + mPlayer.gameObject.transform.rotation * mOffset;
        transform.position = Vector3.Slerp(transform.position, targetPos, mSlerpSpeed * Time.deltaTime);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyVenom");
        //독 데미지 = 데미지 * 0.5(기존 데미지 50%), 죽을때까지 1초 간격 > 그냥 지속시간을 엄청 늘려놨음
        float venomDamage = damage * mDamageDuplicater;
        target.TakeDotDamage(venomDamage * FairyReinforceStatic.FairyAttackDamageDuplicater, mEffectTime, mDamageTick, EDmgElement.Poison);
    }
}
