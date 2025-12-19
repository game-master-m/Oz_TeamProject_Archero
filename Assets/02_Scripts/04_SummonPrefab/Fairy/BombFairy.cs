using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFairy : FairyBase
{
    [SerializeField] private BombFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(0, 1, -3);
    [SerializeField] private Bomb mBombPrefab;
    private float mSlerpSpeed = 5.0f;
    private float mEffectTime;
    private float mDamageTick;
    private float mDamageDuplicater;
    private float mBombRange;

    public void SetUp(BombFairySkillDataSO skillDataSO, PlayerAttack attack)
    {
        SetOwner(attack);

        Managers.Pool.CreatePool(mBombPrefab, 5, Managers.Pool.transform);

        //페어리 데이터 스텟 받아오기
        mSkillData = skillDataSO;
        mSeatNumber = skillDataSO.SeatNumber;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mBombRange = skillDataSO.BombRange;

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
        float bombDamage = damage * mDamageDuplicater;
        
    }
}
