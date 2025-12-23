using System.Collections.Generic;
using UnityEngine;

public class RandomSelectorNode : Node
{
    private List<Node> mChildren;
    private List<Node> mShuffledChildren;
    private int mCurrentRunningIndex = -1; // 현재 실행 중인 노드의 인덱스

    public RandomSelectorNode(List<Node> nodes)
    {
        mChildren = nodes;
        mShuffledChildren = new List<Node>(mChildren);
    }

    public override ENodeState Evaluate()
    {
        // 1. 이미 실행 중인 노드가 있다면 해당 노드만 계속 실행
        if (mCurrentRunningIndex != -1)
        {
            return ProcessChild(mCurrentRunningIndex);
        }

        // 2. 실행 중인 노드가 없다면(새로 시작한다면) 리스트를 섞음
        ShuffleNodes();

        // 3. 순차적으로 실행하며 Running이나 Success를 찾음
        for (int i = 0; i < mShuffledChildren.Count; i++)
        {
            var state = ProcessChild(i);
            if (state != ENodeState.Failure) return state;
        }

        return ENodeState.Failure;
    }

    private ENodeState ProcessChild(int index)
    {
        var state = mShuffledChildren[index].Evaluate();

        if (state == ENodeState.Running)
        {
            mCurrentRunningIndex = index; // 인덱스 기억
        }
        else
        {
            mCurrentRunningIndex = -1; // 종료(S/F) 시 기억 초기화
        }

        return state;
    }

    private void ShuffleNodes()
    {
        for (int i = mShuffledChildren.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            var temp = mShuffledChildren[i];
            mShuffledChildren[i] = mShuffledChildren[rnd];
            mShuffledChildren[rnd] = temp;
        }
    }

    public override void Abort()
    {
        if (mCurrentRunningIndex != -1)
        {
            mShuffledChildren[mCurrentRunningIndex].Abort();
            mCurrentRunningIndex = -1;
        }
        base.Abort();
    }
}