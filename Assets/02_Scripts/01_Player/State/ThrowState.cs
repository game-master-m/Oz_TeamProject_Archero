using System.Collections;
using UnityEngine;

public class ThrowState : PlayerState
{
    //애니메이션이 끝날때 공격하기 -> 애니메이션 중 던질 때 화살생성
    //AnimatorStateInfo.normalizedTime(1)
    // ㄴ 프레임드랍이 생길경우 애니메이션이 느려질 수 있음
    //0에서 시작해서 1에서 끝남(루프 애니메이션의 경우 계속 늘어남. 예, 2번째 반복모션 끝일 땐 2, % 사용하여 제어 가능)

    //플레이의 공격속도를 기반으로 애니메이션 속도조절, 실제 공격타이밍 조절
    public ThrowState(PlayerController player, IState parent = null) : base(player, parent) { }

    //공격속도에 따른 1번 공격에 걸리는 총 시간
    private float mDurationPerOneShot;

    //애니메이션 파라미터 값
    private float mAttackSpeedMultiplier;

    //모션이 끝날 때 던지는 것이 아닌, 모션 중 던졌을 때, 화살을 발사하기 위한 딜레이
    private readonly float mFireDelayNormalized = 0.565f;   //0~1 사이 비율값
    private readonly float mClipLength = 1.367f;

    //코루틴 최적화
    private Coroutine mRunningCo = null;
    private WaitForSeconds mWaitPerOneShot;
    public override void Enter()
    {
        Utils.Log("Throw Enter");
        //공속에 따른 한발당 소요되는 시간
        mDurationPerOneShot = 1 / mPlayer.Stat.AttackSpeed;
        mWaitPerOneShot = new WaitForSeconds(mDurationPerOneShot);
        //원본 애니메이션 속도배율 계산
        mAttackSpeedMultiplier = mClipLength / mDurationPerOneShot;
        mPlayer.Anim.SetFloat(AnimHash.attackSpeedMultiplier, mAttackSpeedMultiplier);

        //알맞은 던지기 모션 시간계산
        float fireDelay = mDurationPerOneShot * mFireDelayNormalized;

        if (mRunningCo == null)
        {
            mRunningCo = mPlayer.StartCoroutine(MakeProjectileCo(fireDelay, mDurationPerOneShot));
        }
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        if (mRunningCo != null)
        {
            mPlayer.StopCoroutine(mRunningCo);
            mRunningCo = null;
        }
        if (mPlayer.CheckEnemyInRangeCo != null)
        {
            mPlayer.StopCoroutine(mPlayer.CheckEnemyInRangeCo);
            mPlayer.CheckEnemyInRangeCo = null;
        }
    }

    private IEnumerator MakeProjectileCo(float fireDelay, float durationPerOneShot)
    {
        //첫 발사는 후딜레이만큼의 시간을 뺀 후, 애니메이션 재생
        yield return new WaitForSeconds(durationPerOneShot - fireDelay);
        mPlayer.Anim.CrossFade(AnimHash._throw, 0.1f);
        //남은 시간 대기하고 발사
        yield return new WaitForSeconds(fireDelay);
        mPlayer.Attack.MakeProjectile();
        while (true)
        {
            //애니메이션은 계속 실행되기때문에 durationPerOneShot 시간마다 화살생성
            yield return mWaitPerOneShot;
            mPlayer.Attack.MakeProjectile();
        }
    }
}
