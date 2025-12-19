using UnityEngine;

public enum BT_NodeStatus
{
    Sucess,
    Failure,
    Running
}
public abstract class BT_Node
{
    public abstract BT_NodeStatus Evaluate();
}
