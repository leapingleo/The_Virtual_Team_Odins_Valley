using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    /* Children nodes that belong to this sequence */
    private List<Node> nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    /* If any child node returns a failure, the entire node fails.
     * If all child nodes report a success, the node reports a success.
     */
    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;
        NodeState childNodeState = NodeState.NULL;

        foreach (Node node in nodes)
        {
            childNodeState = node.Evaluate();

            if (childNodeState == NodeState.FAILURE)
            {
                nodeState = NodeState.FAILURE;
                return nodeState;
            }

            /*
             * We ignore the fact if the child is a success, we only care
             * to check if child is a failure.
             */

            if (childNodeState == NodeState.RUNNING)
            {
                anyChildRunning = true;
            }
        }

        /*
         * If we are able to reach the end of all the child nodes, the we know this 
         * node is still running or ultimately successful. 
         */
        nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;

        /*
         * A success state means we live to fight another day and can move on to the next
         * child node. A running state essentially means we still need reevaluate this node before
         * we can determine if it is a success or not. 
         */
        return nodeState;
    }

}
