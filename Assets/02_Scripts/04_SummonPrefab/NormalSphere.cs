using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSphere : SphereBase
{
    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetUp(mPlayer);
    }

    void Update()
    {
        this.gameObject.transform.position = mPlayer.gameObject.transform.position + mPositionOffset;
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        float sphereDamage = damage * 1.25f;

        Utils.Log($"구체데미지{sphereDamage}");
        //target.TakeDamage(sphereDamage);
    }
}
