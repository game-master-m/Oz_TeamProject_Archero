using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSphere : SphereBase
{
    [SerializeField] private NormalSphereSkillDataSO mSkillDataSO;


    private void OnEnable()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillDataSO);
    }
    void Update()
    {
        this.gameObject.transform.position = mPlayer.gameObject.transform.position + mPositionOffset;
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public void SetUp(NormalSphereSkillDataSO skillDataSO)
    {
        mTotalCount--;
        mPositionOffset = skillDataSO.PositionOffset;
        mRotateSpeed = skillDataSO.RotateSpeed;
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        float sphereDamage = damage * 0.9f;

        Utils.Log($"구체데미지{sphereDamage}");
        target.TakeDamage(sphereDamage);
    }
}
