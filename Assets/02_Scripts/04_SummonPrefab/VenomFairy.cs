using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomFairy : FairyBase
{
    [SerializeField] private VenomFairySkillDataSO mSkillData;

    //스타트는 테스트 환경에서 작동 확인용 세팅
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillData);
    }

    public void SetUp(VenomFairySkillDataSO skillDataSO)
    {
        //페어리 데이터 스텟 받아오기
        mEffectTime = skillDataSO.mEffectTime;
        mDamageTick = skillDataSO.mDamageTick;
        mDamageDuplicater = skillDataSO.mDamageDuplicater;
        mSeatNumber = skillDataSO.mSeatNumber;

        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up,mSeatAngle * mSeatNumber);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyVenom");
        //독 데미지 = 데미지 * 0.5(기존 데미지 50%), 죽을때까지 1초 간격 > 그냥 지속시간을 엄청 늘려놨음
        float venomDamage = damage * mDamageDuplicater;
        target.TakeDotDamage(venomDamage, mEffectTime, mDamageTick);
    }
}
