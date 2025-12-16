using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFairy : FairyBase
{
    [SerializeField]private FireFairySkillDataSO mSkillData;

    //스타트는 테스트 환경에서 작동 확인용 세팅
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillData);
    }

    public void SetUp(FireFairySkillDataSO skillDataSO) 
    {
        //페어리 데이터 스텟 받아오기
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mSeatNumber = skillDataSO.SeatNumber;

        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();

        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyFire");
        //화염 데미지 = 데미지 * 0.2(기존 데미지 20%), 3초동안 0.2초 간격
        float fireDamage = damage * mDamageDuplicater;
        target.TakeDotDamage(fireDamage, mEffectTime, mDamageTick);
    }
}
