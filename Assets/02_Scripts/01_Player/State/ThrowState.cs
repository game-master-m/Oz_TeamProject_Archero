using UnityEngine;

public class ThrowState : PlayerState
{
    //애니메이션이 끝날때 공격하기
    //AnimatorStateInfo.normalizedTime(1)
    //0에서 시작해서 1에서 끝남
    

    private float mAttackCooldown=1.0f;
    private float mAttackTimer = 0.0f;

    private PlayerStatDataSO mStat;
    public ThrowState(PlayerController player,PlayerStatDataSO stat, IState parent = null) : base(player, parent)
    {
        mStat = stat;
    }

    public override void Enter()
    {
        Utils.Log("Throw Enter");
        mPlayer.Anim.CrossFade(AnimHash._throw, 0.1f);
            

    }
    public override void Update()
    {
        if (!EnemyInRange())
        {
            mPlayer.StateMachine.ChangeState(mPlayer.StopState);


            return;
        }
        Move();
        ShootArrow();

        
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
    private bool EnemyInRange()
    {
        float range = mStat.AttackRange;

        Collider[] enemy = Physics.OverlapSphere(mPlayer.transform.position, range, Layers.GetLayerMask(ELayerName.Enemy));
        return enemy.Length > 0;
    }
    private void ShootArrow()
    {
        //초당1번씩
        mAttackTimer += Time.deltaTime;

        if (mAttackTimer < mAttackCooldown) return;
        mAttackTimer = 0f;

        mPlayer.Attack.MakeProjectile();
    }
    private void ShootArrow2()
    {
        //AnimatorStateInfo.normalizedTime
    }
    private void Move()
    {
        
        if (mPlayer.InputDir.sqrMagnitude > 0.01f)
        {
            mPlayer.StateMachine.ChangeState(mPlayer.StopState);
            return;
        }
    }
    

}
