using UnityEngine;

public class WaitNode : ActionNode
{
    private float mStartTime;
    private bool bIsWaiting = false;
    private float mWaitTime;
    private bool bIsCheckAround;

    private Quaternion mStartRotation;
    private readonly float mLookAroundAngle = 40.0f;
    private readonly float mLookAroundSpeed = 1.5f;

    public WaitNode(EnemyBase owner, float waitTime, bool isCheckAround = true) : base(owner)
    {
        mWaitTime = waitTime;
        bIsCheckAround = isCheckAround;
    }

    public override ENodeState Evaluate()
    {
        if (!bIsWaiting)
        {
            mStartTime = Time.time;
            bIsWaiting = true;

            mStartRotation = mOwner.transform.rotation;

            // 보드에 저장된 랜덤 시간을 사용합니다.
            Debug.Log($"드래곤이 {mWaitTime:F1}초 동안 대기합니다.");
        }
        float elapsed = Time.time - mStartTime;
        if (elapsed >= mWaitTime)
        {
            bIsWaiting = false;
            return ENodeState.Success;
        }

        if (bIsCheckAround)
        {
            //두리번 거리는 효과를 위해 좌우로 회전
            // Sine 함수를 이용해 -1 ~ 1 사이를 왕복하는 값 생성
            float sinValue = Mathf.Sin(elapsed * mLookAroundSpeed);

            // 시작 회전값에서 Y축을 기준으로 좌우 회전 연산
            Quaternion lookRotation = Quaternion.Euler(0, sinValue * mLookAroundAngle, 0);

            // Slerp를 사용하여 부드럽게 회전 적용
            mOwner.transform.rotation = Quaternion.Slerp(
                mOwner.transform.rotation,
                mStartRotation * lookRotation,
                mOwner.RotateSpeed * Time.deltaTime
            );
        }

        return ENodeState.Running;
    }

    public override void Abort()
    {
        bIsWaiting = false;
        base.Abort();
    }
}