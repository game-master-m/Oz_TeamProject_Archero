using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFairy : FairyBase
{
    [SerializeField] private BombFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(0, 1, -3);
    [SerializeField] private Vector3 mBombOffset = new Vector3(0, 2, 0);
    [SerializeField] private Bomb mBombPrefab;
    private Bomb mBomb;

    private float mSlerpSpeed = 5.0f;
    private float mBombDamage;

    private float mBombRange;
    private float mBombHeight;
    private float mBombCount;
    private float mBombThrowTick;

    private WaitForSeconds mWaitThrowTick;

    public void SetUp(BombFairySkillDataSO skillDataSO, PlayerAttack attack)
    {
        SetOwner(attack);

        Managers.Pool.CreatePool(mBombPrefab, 5, Managers.Pool.transform);

        //페어리 데이터 스텟 받아오기
        mSkillData = skillDataSO;
        mSeatNumber = skillDataSO.SeatNumber;
        mDamageDuplicater = skillDataSO.DamageDuplicater;

        mBombRange = skillDataSO.BombRange;
        mBombHeight = skillDataSO.BombHeights;
        mBombCount = skillDataSO.BombCount;
        mBombThrowTick = skillDataSO.BombThrowTick;

        mWaitThrowTick = new WaitForSeconds(mBombThrowTick);

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
        mBombDamage = damage * mDamageDuplicater;

        StartCoroutine(BombThrowCo());
    }

    public void Explode(EnemyBase target) 
    {
        //폭탄 데미지 = 데미지 * 0.4(기존 데미지 40%) * 다섯번
        target.TakeDamage(mBombDamage * FairyReinforceStatic.FairyAttackDamageDuplicater, EDmgElement.Normal);
    }

    IEnumerator BombThrowCo() 
    {
        for (int i = 0; i < mBombCount; i++)
        {
            mBomb = Managers.Pool.GetFromPool(mBombPrefab);
            mBomb.SetUp(this.gameObject, mBombHeight, mBombRange);
            mBomb.gameObject.transform.position = this.gameObject.transform.position + mBombOffset;
            LookTarget();
            mBomb.DoJump(mTargetTransform.position, mBombHeight);
            yield return mWaitThrowTick;
        }
    }
}
