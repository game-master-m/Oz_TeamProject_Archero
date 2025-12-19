using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float mMoveSpeed = 24f;

    private Vector3 mStartPos;
    private Vector3 mEndPos;
    private float mJumpHeight;

    private float mElapsedTime = 0;
    private float mTotalTime = 0;
    private bool mIsJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsJumping) 
        {
            mElapsedTime += Time.deltaTime;

            Vector3 horizontalPos = Vector3.Lerp(mStartPos, mEndPos, mElapsedTime/ mTotalTime);

            float verticalPos = mJumpHeight * 4 * (mElapsedTime / mTotalTime) * (1 - mElapsedTime / mTotalTime);

            transform.position = new Vector3(horizontalPos.x, mStartPos.y + verticalPos, horizontalPos.z);

            if (mElapsedTime >= mTotalTime) 
            {
                mIsJumping = false;
            }
        }
    }

    //y = 4 * height * (경과시간/전체점프시간) * (1 - 경과시간/전체점프시간)
    public void DoJump(Vector3 start, Vector3 target, float height) 
    {
        mStartPos = start;
        mEndPos = target;
        mJumpHeight = height;
        transform.Translate(0, -transform.position.y, 0);

        float horizontalDistance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(target.x, 0, target.z));

        mTotalTime = horizontalDistance / mMoveSpeed;
        mElapsedTime = 0f;
        
        mIsJumping = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
