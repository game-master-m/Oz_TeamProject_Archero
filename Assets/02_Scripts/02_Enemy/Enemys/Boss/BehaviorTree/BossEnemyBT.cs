using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBT : MonoBehaviour
{
    private BT_Node root;

    public Transform target;
    public float chaseDistance = 5.0f;
    public float attackDistance = 2.5f;
    public float speed = 2.0f;


    Animator mAnim;


    private float mSpreadCount = 5.0f;
    private float mSpreadDeg = 30.0f;


    //우선 공격만 해야된다

    private void Awake()
    {
        mAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        //root=최상단 노드
        root = new BT_Selector(new List<BT_Node>
        {
            //1.직선공격관련 시퀀스
            new BT_Sequence(new List<BT_Node>
            {
                new BT_Leaf(CheckPlayerInRange),
                new BT_Leaf(AttackPlayer),
            }),
            //2.사선공격관련 시퀀스
            new BT_Sequence(new List<BT_Node>
            {
                new BT_Leaf(CheckChaseRange),
                new BT_Leaf(ChasePlayer),
            }),

            new BT_Leaf(Idle)
        });

        //root = new BT_Selector(new List<BT_Node>
        //{
        //    //1.공격관련 시퀀스
        //    new BT_Sequence(new List<BT_Node>
        //    {
        //        new BT_Leaf(CheckPlayerInRange),
        //        new BT_Leaf(AttackPlayer),
        //    }),
        //    //2.추격관련 시퀀스
        //    new BT_Sequence(new List<BT_Node>
        //    {
        //        new BT_Leaf(CheckChaseRange),
        //        new BT_Leaf(ChasePlayer),
        //    }),

        //    new BT_Leaf(Idle)
        //});
    }
    void Update()
    {
        root.Evaluate();
    }
    //private BT_NodeStatus를 이용해서 공격패턴만들기
    private BT_NodeStatus Spread(Vector2 pos, Vector2 dir, float speed)
    {
        float half = mSpreadCount / 2;

        for (float i = -half; i <= half; i++)
        {
            float angle = i * mSpreadDeg;
            Vector2 newDir = Quaternion.Euler(0, 0, angle) * dir;

        }
        return BT_NodeStatus.Sucess;
    }


    //범위 체크
    private BT_NodeStatus RangeCheck(float range)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < range ? BT_NodeStatus.Sucess : BT_NodeStatus.Failure;
    }
    //추격거리 안에 있는지
    BT_NodeStatus CheckChaseRange()
    {
        return RangeCheck(chaseDistance);
    }
    //공격거리 안에 있는지
    BT_NodeStatus CheckPlayerInRange()
    {
        return RangeCheck(attackDistance);
    }
    //Leaf행동들(실제 애니/이동로직)
    BT_NodeStatus Idle()
    {

        AnimatorChange("IDLE");
        return BT_NodeStatus.Sucess;
    }
    BT_NodeStatus AttackPlayer()
    {
        Rotate();
        AnimatorChange("ATTACK");
        return BT_NodeStatus.Sucess;
    }
    BT_NodeStatus ChasePlayer()
    {
        Rotate();
        AnimatorChange("MOVE");
        return BT_NodeStatus.Running;
    }
    private void AnimatorChange(string temp)
    {

        mAnim.SetBool("IDLE", false);
        mAnim.SetBool("MOVE", false);
        mAnim.SetBool("ATTACK", false);

        mAnim.SetBool(temp, true);
    }
    private void Rotate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction == Vector3.zero) return;
        transform.forward = direction;
    }

}
