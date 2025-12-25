using System;
using System.Collections.Generic;
using UnityEngine;

public static class BT_Builder
{
    //Patrol & Idle
    public static SequenceNode GetPatrolBT(EnemyBase enemy, BlackBoard board, float patrolRange, float minWaitTime, float maxWaitTime)
    {
        Node setup = new SetRandomPatrolDataNode(enemy, board, patrolRange, minWaitTime, maxWaitTime);

        // 2. 이동 (보드에 설정된 LastKnownPos 사용)
        Node move = new CMoveToNextPosNode(enemy, board);

        // 3. 대기 (보드에 설정된 CurrentWaitTime 사용)
        Node wait = new CWaitNode(enemy, 1.0f);

        // 4. 순차 실행 (메모리 기능을 켜서 이동 중 중단되어도 이어서 진행)
        SequenceNode seq = new SequenceNode(new List<Node> { setup, move, wait }, true);

        return seq;
    }

    //NormalAttack
    public static SequenceNode GetNormalAttackBT(EnemyBase enemy, BlackBoard board, float rotateSpeed, float hitTiming, float hitBoxOffsetForward, float hitBoxRadius, float waitTime)
    {
        SequenceNode normalAttack = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => (enemy.transform.position - board.Target.position).sqrMagnitude <= enemy.AttackRange*enemy.AttackRange),
            new RotateToTargetNode(enemy,board,rotateSpeed),
            new CAttackNode(enemy, hitTiming, hitBoxOffsetForward, hitBoxRadius),
            new CWaitNode(enemy, waitTime, false),
        });
        return normalAttack;
    }

    //NormalShot
    public static SequenceNode GetNormalShotBT(EnemyBase enemy, BlackBoard board, float rotateSpeed, float shotTiming, float waitTime, float projectileSpeed, Vector3 offset, Func<EnemyProjectileBase> factory)
    {
        SequenceNode normalShot = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => (enemy.transform.position - board.Target.position).sqrMagnitude <= enemy.AttackRange*enemy.AttackRange),
            new RotateToTargetNode(enemy,board,rotateSpeed),
            new CNormalShotNode(enemy,board,projectileSpeed,shotTiming,1.0f,offset,factory),
            new CWaitNode(enemy, waitTime, false),
        });
        return normalShot;
    }

    //Chase And Attack
    public static SelectorNode GetChaseAndAttackBT(EnemyBase enemy, BlackBoard board, float rotateSpeed, float hitTiming, float hitBoxOffsetForward, float hitBoxRadius, float waitTime)
    {
        SelectorNode select = new SelectorNode(new List<Node>
        {
            GetNormalAttackBT(enemy,board,rotateSpeed,hitTiming,hitBoxOffsetForward,hitBoxRadius,waitTime),
            new SequenceNode(new List<Node>()
            {
                new CWaitNode(enemy,0.5f,true),
                new CMoveToTargetNode(enemy,board),
            })
        });
        return select;
    }

    //Chase And Shot
    public static SelectorNode GetChaseAndShotBT(EnemyBase enemy, BlackBoard board, float rotateSpeed, float shotTiming, float waitTime, float projectileSpeed, Vector3 offset, Func<EnemyProjectileBase> factory)
    {
        SelectorNode select = new SelectorNode(new List<Node>
        {
            GetNormalShotBT(enemy,board, rotateSpeed,shotTiming,waitTime,projectileSpeed,offset,factory),
            new SequenceNode(new List<Node>()
            {
                new CWaitNode(enemy,0.5f,true),
                new CMoveToTargetNode(enemy,board),
            })
        });
        return select;
    }
}
