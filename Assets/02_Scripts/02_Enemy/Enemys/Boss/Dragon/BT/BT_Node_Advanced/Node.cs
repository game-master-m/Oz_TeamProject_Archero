
public abstract class Node
{
    public ENodeState State { get; protected set; }

    // 매 프레임 실행될 로직
    public abstract ENodeState Evaluate();

    // 실행 중인 노드를 강제로 멈출 때 호출
    public virtual void Abort()
    {
        State = ENodeState.Failure;
    }
}