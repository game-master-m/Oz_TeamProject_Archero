
using Unity.VisualScripting;
using UnityEngine;
public class StopState : PlayerState
{

    private float mTimer = 0.0f;

    private PlayerStatDataSO mStat;
    
    
    public StopState(PlayerController player,PlayerStatDataSO stat,IState parent = null) : base(player, parent)
    {
        
        mStat = stat;
    }
    public override void Enter()//Stop 애니메이션일때 한번 실행
    {
        
        
        


        //유니티 에디터에서만 로그찍기
        Utils.Log("Stop Enter");
        //애니메이션 전환( CrossFade(clip name, 전환시간) , Play(clip name) )
        mPlayer.Anim.CrossFade(AnimHash.idle, 0.1f);

        //테스트 쏘기
        //if (!mPlayer.Attack.IsAutoTurret)
        //{
        //    mPlayer.Attack.MakeProjectile();
        //}

    }
    
    public override void Update()
    {
        //PlayerStatDataSO의 mAttackRange를 가져와서 이 사거리 안에 Layer Enemy가 있으면 throwState(mPlayer.Anim.CrossFade(AnimHash._throw, 0.1f);)로 변경        


        //Collider[] hitColliders = Physics.OverlapSphere(mPlayer, mStat.AttackRange, Layers.GetLayerMask(ELayerName.Enemy));

        //if (Collider[]hitcollider=Physics.OverlapSphere())
        //mStat.AttackRange
        base.Update();
        
        if (EnemyInRange())
        {
            mPlayer.StateMachine.ChangeState(mPlayer.ThrowState);
                       
            
            return;
        }

        

    }
    public override void FixedUpdate() { }
    public override void Exit() { }
    private bool EnemyInRange()
    {
        float range = mStat.AttackRange;

        Collider[] enemy = Physics.OverlapSphere(mPlayer.transform.position, range, Layers.GetLayerMask(ELayerName.Enemy));
        return enemy.Length > 0;
    }


}
