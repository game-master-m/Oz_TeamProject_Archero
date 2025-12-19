using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float mMoveSpeed = 24f;

    private GameObject mOwner;

    private Vector3 mStartPos;
    private Vector3 mEndPos;
    private float mJumpHeight;
    private float mBombRange;

    private float mElapsedTime = 0;
    private float mTotalTime = 0;
    private bool mIsJumping = false;

    // Update is called once per frame
    void Update()
    {
        if (mIsJumping)
        {
            mElapsedTime += Time.deltaTime;

            Vector3 horizontalPos = Vector3.Lerp(mStartPos, mEndPos, mElapsedTime / mTotalTime);

            float verticalPos = mJumpHeight * 4 * (mElapsedTime / mTotalTime) * (1 - mElapsedTime / mTotalTime);

            transform.position = new Vector3(horizontalPos.x, mStartPos.y + verticalPos, horizontalPos.z);

            if (mElapsedTime >= mTotalTime)
            {
                mIsJumping = false;
                Explode();
            }
        }
    }

    public void SetUp(GameObject owner, float height, float range)
    {
        mOwner = owner;
        mJumpHeight = height;
        mBombRange = range;
    }

    //y = 4 * height * (경과시간/전체점프시간) * (1 - 경과시간/전체점프시간)
    public void DoJump(Vector3 target, float height)
    {
        mStartPos = this.gameObject.transform.position;
        mEndPos = target;
        mJumpHeight = height;
        transform.Translate(0, -transform.position.y, 0);

        float horizontalDistance
            = Vector3.Distance(new Vector3(mStartPos.x, 0, mStartPos.z), new Vector3(mEndPos.x, 0, mEndPos.z));

        mTotalTime = horizontalDistance / mMoveSpeed;
        mElapsedTime = 0f;

        mIsJumping = true;
    }

    private void Explode()
    {
        if (!mOwner.TryGetComponent(out BombFairy fairy)) 
        {
            return;
        }
        var owner = fairy;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, mBombRange, Layers.GetLayerMask(ELayerName.Enemy));

        foreach (Collider hitCollider in hitColliders)
        {
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            if (hitCollider.TryGetComponent(out EnemyBase enemy)) 
            {
                owner.Explode(enemy);
            }
        }

        Managers.Pool.ReturnToPool(this);
    }
}