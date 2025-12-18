using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class VampireCircle : SphereBase
{
    [SerializeField] private VampireCircleSkillDataSO mSkillDataSO;
    [SerializeField] private int mOrbitIndex = 0;
    private int mOrbitCount = 0;

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

    public void SetUp(VampireCircleSkillDataSO skillDataSO)
    {
        mOrbitCount = mTotalCount;
        mOrbitIndex = mTotalCount / 2;
        mPositionOffset = skillDataSO.PositionOffset;
        mRotateSpeed = skillDataSO.RotateSpeed;
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        //float sphereDamage = damage * 1.25f;
        float restoreAmount = mPlayer.Stat.MaxHP * 0.02f;
        mPlayer.Stat.AddHP(restoreAmount);
        Utils.Log($"체력{restoreAmount}만큼회복");
    }
}
