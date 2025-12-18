using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFairy : FairyBase
{
    [SerializeField] private LaserFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(-2, 1, -3);

    [SerializeField] private LaserEffect mLaserEffectPrefab;
    [SerializeField] private float mLaserRadius = 2.0f;
    private LaserEffect mLaserEffect;

    private float mTimer = 0f;
    private float mSlerpSpeed = 5.0f;

    private float mLaserDuration;
    private float mLaserDamageTick;
    private float mLaserRange;
    private float mLaserDamageDuplicater;

    private bool mIsAttack = false;

    private WaitForSeconds mLaserDelay;
    //스타트는 테스트 환경에서 작동 확인용 세팅
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillData);
    }

    public void SetUp(LaserFairySkillDataSO skillDataSO)
    {
        Managers.Pool.CreatePool(mLaserEffectPrefab, 10, Managers.Pool.transform);

        //페어리 데이터 스텟 받아오기
        mSeatNumber = skillDataSO.SeatNumber;
        mLaserDuration = skillDataSO.LaserDuration;
        mLaserDamageTick = skillDataSO.LaserDamageTick;
        mLaserRange = skillDataSO.LaserRange;
        mLaserDamageDuplicater = skillDataSO.LaserDamageDuplicater;

        mLaserDelay = new WaitForSeconds(mLaserDamageTick);

        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    private void Update()
    {
        if (!mIsAttack)
        {
            mTimer += Time.deltaTime;
            if (mTimer >= mAttackDelay)
            {
                mTimer = 0;
                LaserAttack();
            }
        }
        else
        {
            if (mLaserEffect == null)
            { 
                mLaserEffect = Managers.Pool.GetFromPool(mLaserEffectPrefab); 
            }
            Vector3 start = this.gameObject.transform.position;
            Vector3 end = start + this.gameObject.transform.forward * mLaserRange;
            mLaserEffect.SetLineRenderer(mLaserRadius);
            mLaserEffect.DrawLaser(start, end);
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mPlayer.gameObject.transform.position + mPlayer.gameObject.transform.rotation * mOffset;
        transform.position = Vector3.Slerp(transform.position, targetPos, mSlerpSpeed * Time.deltaTime);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        if (LookTarget())
        {
            StartCoroutine(LaserAttackCo());
        }
    }

    private void LaserAttack() 
    {
        if (LookTarget()) 
        {
            mIsAttack = true;
            StartCoroutine(LaserAttackCo());
        }
    }

    private IEnumerator LaserAttackCo() 
    {
        float startTime = Time.time;
        float laserDamage = mPlayer.Stat.AttackDamage * mLaserDamageDuplicater;

        while (Time.time - startTime < mLaserDuration) 
        {
            RaycastHit[] hit = Physics.SphereCastAll(this.gameObject.transform.position, mLaserRadius, this.gameObject.transform.forward, mLaserRange);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.TryGetComponent(out EnemyBase enemy)) 
                {
                    enemy.TakeDamage(laserDamage);
                }
            }
            yield return mLaserDelay;
        }
        mLaserEffect.gameObject.SetActive(false);
        Managers.Pool.ReturnToPool(mLaserEffect);
        mLaserEffect = null;
        mIsAttack = false;
        Utils.Log($"레이저 공격 : {mIsAttack}");
    }
}
