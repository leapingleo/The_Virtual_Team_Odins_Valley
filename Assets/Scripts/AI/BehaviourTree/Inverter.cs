using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    /* Child node to evaluate */
    private Node childNode;

    public Node ChildNode { get { return childNode; } }

    /* The constructor requires the child node that this invertor decorator wraps */
    public Inverter (Node childNode)
    {
        this.childNode = childNode;
    }

    /* Reports a success if the child fails and a failure if the
     * child succeeds. Running will report as running.
     */
    public override NodeState Evaluate()
    {
        NodeState childNodeState = childNode.Evaluate();

        if (childNodeState == NodeState.SUCCESS)
        {
            nodeState = NodeState.FAILURE;
        }
        else if (childNodeState == NodeState.FAILURE)
        {
            nodeState = NodeState.SUCCESS;
        }
        else if (childNodeState == NodeState.RUNNING)
        {
            nodeState = NodeState.RUNNING;
        }

        return nodeState;
    }


}
