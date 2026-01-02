using UnityEngine;

public class BossAttackState : EnemyState
{
    
    private float mNextAttackTime;
    private Quaternion targetRot;


    

    [SerializeField] private GameManager mBulletPrefab;//총알 프리팹
    [SerializeField] private float mSpreadCount = 5.0f;
    [SerializeField] private float mSpreadDeg = 30.0f;


    private Transform mTarget;//미사일이 날아갈 타겟
    


    


    public BossAttackState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }
    public override void Enter()
    {
        base.Enter();
        mNextAttackTime = 0f;
        // 이동 멈추기
        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
            mEnemy.Agent.isStopped = true;
            mEnemy.Agent.velocity = Vector3.zero;
        }
        //공격 애니메이션 재생
        //애니메이션 파라미터는 최대한 안 쓰려고 함.
        //mEnemy.Anim.SetBool("IsAttack", true);
        mEnemy.Anim.CrossFade(AnimHash.attack, 0.1f);
    }
    public override void Update()
    {
        base.Update();
        //여기에 근접 공격 상태에서 필요한 로직
        //투사체를 발사한다거나, 공격 타이밍을 조절한다거나 

        if (mEnemy.Target == null) return;

        Vector3 dir = (mEnemy.Target.position - mEnemy.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            //회전값 보정(곱하는 순서가 중요, Forward까지의 회전값 * 보정 회전값)
            Quaternion targetRot = lookRot * mEnemy.CorrectionQtrn;
            mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, targetRot, mEnemy.RotateSpeed * Time.deltaTime);
        }

        // 공격 시도
        if (Time.time >= mNextAttackTime)
        {
            mNextAttackTime = Time.time + mEnemy.AttackSpeed;
            PerformAttack();
        }
    }
    private void PerformAttack()
    {
        //공격 로직 구현
        Utils.Log("Melee Attack Performed");
        //여기에 실제 공격 판정 및 데미지 적용 로직을 추가하세요.
        {
            //애니메이션 파라미터는 최대한 안 쓰려고 함.
            //mEnemy.Anim.SetTrigger("AttackTrigger");

            if (mEnemy.Target == null) return;
            LivingEntity targetEntity = mEnemy.Target.GetComponent<LivingEntity>();

            if (targetEntity != null && !targetEntity.IsDead)
            {
                targetEntity.TakeDamage(mEnemy.AttackDamage);
            }
        }
    }
    private void SpreadShot(Vector2 pos,Vector2 dir,float speed)
    {
        float half = mSpreadCount / 2;

        for(float i = -half; i <= half; i++)
        {
            float angle = i * mSpreadDeg;
            Vector2 newDir=Quaternion.Euler(0,0,angle)*dir;

        }
    }
    
    public override void Exit()
    {
        base.Exit();
        // 공격이 끝났을 때 필요한 정리 작업들
        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
            mEnemy.Agent.isStopped = false;
        }
    }
}
