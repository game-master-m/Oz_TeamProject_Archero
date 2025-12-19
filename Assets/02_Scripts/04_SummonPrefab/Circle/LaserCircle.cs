using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserCircle : SphereBase
{
    [SerializeField] private LaserCircleSkillDataSO mSkillDataSO;

    [SerializeField] private int mOrbitIndex = 0;
    private int mOrbitCount = 0;

    void Update()
    {
        if (mOrbitCount != mTotalCount)
        {
            if (mTotalCount <= 1) { mTotalCount = 2; }
            float angle = (180 / (mTotalCount / 2)) * mOrbitIndex;
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
            mOrbitCount = mTotalCount;
        }
        this.gameObject.transform.position = mPlayer.gameObject.transform.position + mPositionOffset;
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public void SetUp(LaserCircleSkillDataSO skillDataSO)
    {
        mOrbitCount = mTotalCount;
        mOrbitIndex = mTotalCount / 2;
        mPositionOffset = skillDataSO.PositionOffset;
        mRotateSpeed = skillDataSO.RotateSpeed;

        foreach (var sphere in mChildShpheres) 
        {
            if (sphere.TryGetComponent(out LaserSphere laserSphere)) 
            {
                laserSphere.SetLaser(skillDataSO, mPlayer);
            }
        }
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        float sphereDamage = damage * 1.25f;

        Utils.Log($"구체데미지{sphereDamage}");
        target.TakeDamage(sphereDamage);
    }
}
