using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class VenomCircle : SphereBase
{
    [SerializeField] private VenomCircleSkillDataSO mSkillDataSO;

    [SerializeField]private int mOrbitIndex = 0;
    private int mOrbitCount = 0;
    private float mEffectTime;
    private float mDamageTick;
    private float mDamageDuplicater;

    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillDataSO);
    }

    void Update()
    {
        if (mOrbitCount != mTotalCount)
        {
            float angle = (180 / (mTotalCount / 2)) * mOrbitIndex;
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
            mOrbitCount = mTotalCount;
        }
        this.gameObject.transform.position = mPlayer.gameObject.transform.position + mPositionOffset;
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public void SetUp(VenomCircleSkillDataSO skillDataSO)
    {
        mOrbitCount = mTotalCount;
        mOrbitIndex = mTotalCount/2;
        mPositionOffset = skillDataSO.PositionOffset;
        mRotateSpeed = skillDataSO.RotateSpeed;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        float sphereDamage = damage * 1.25f;
        float venomDamage = damage * mDamageDuplicater;

        Utils.Log($"구체데미지{sphereDamage}");
        target.TakeDamage(sphereDamage);
        target.TakeDotDamage(venomDamage, mEffectTime, mDamageTick);
    }
}
