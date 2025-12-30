using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBall : EnemyProjectileBase
{
    [Header("웨이브 이동 세팅용")]
    [SerializeField] private float mWaveWidth = 10.0f;
    [SerializeField] private float mWaveFrequency = 20000.0f;
    private float mWaveTimer = 0f;

    protected override void Awake()
    {
        mRigid = GetComponent<Rigidbody>();
        mRigid.useGravity = false;
        mRigid.isKinematic = true;
    }

    protected override void Update()
    {
        base.Update();

        mWaveTimer += Time.deltaTime;

        Vector3 forward = transform.forward * mMoveSpeed * Time.deltaTime;

        float wave = Mathf.Sin(mWaveTimer * mWaveFrequency) * mWaveWidth;
        Vector3 waveOffset = transform.right * wave * Time.deltaTime;

        transform.position += forward + waveOffset;

        Vector3 moveDir = forward + waveOffset;
        if (moveDir.sqrMagnitude > 0.01f) 
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    protected override void MoveAndRotate()
    {
   
    }
}
