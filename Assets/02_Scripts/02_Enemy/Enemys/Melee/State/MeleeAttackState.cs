using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : EnemyState
{
    private float mNextAttackTime;
    private Quaternion targetRot;

    public MeleeAttackState(MeleeEnemy meleeEnemy, IState parent = null)
        : base(meleeEnemy, parent)
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
        mEnemy.Anim.SetBool("IsAttack", true);
    }

    public override void Update()
    {
        base.Update();
        //여기에 근접 공격 상태에서 필요한 로직
        //투사체를 발사한다거나, 공격 타이밍을 조절한다거나 

        if(mEnemy.Target == null)
            return;

        Vector3 dir = (mEnemy.Target.position - mEnemy.transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lockRotation = Quaternion.LookRotation(dir);
            mEnemy.transform.rotation = Quaternion.Slerp(mEnemy.transform.rotation, targetRot, mEnemy.RotateSpeed*Time.deltaTime);
        }
        float distance = Vector3.Distance(mEnemy.transform.position, mEnemy.Target.position);

        //공격 범위를 벗어나면 MoveState로 전환
        if (distance > mEnemy.AttackRange + 0.5f)
        { 
         mEnemy.Anim.SetBool("IsAttack", false);// 공격 애니메이션 중지
            return;
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
            mEnemy.Anim.SetTrigger("AttackTrigger");

            if (mEnemy.Target == null) return;
            LivingEntity targetEntity = mEnemy.Target.GetComponent<LivingEntity>();

            if (targetEntity != null && !targetEntity.IsDead)
            {
                targetEntity.TakeDamage(mEnemy.AttackDamage);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        // 공격이 끝났을 때 필요한 정리 작업들
        if (mEnemy.Agent != null && mEnemy.Agent.isOnNavMesh)
        {
          mEnemy.Agent.isStopped = false;
        }
        mEnemy.Anim.SetBool("IsAttack", false);// 공격 애니메이션 중지
    }
}
