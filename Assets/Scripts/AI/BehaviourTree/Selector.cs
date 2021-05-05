using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    /* Children of the selector */
    protected List<Node> nodes = new List<Node>();

    /* The constructor requires child nodes to be passed in */
    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    /* If any of the children have a success, the selector will immediately
     * report a success up the tree. If all children fail, it will report
     * a failure instead
     */
    public override NodeState Evaluate()
    {
        NodeState childNodeState = NodeState.NULL;
        foreach (Node node in nodes)
        {
            childNodeState = node.Evaluate();
            /*
             * Since this is a game, performance is crucial so I decided to make it return
             * when finding the first child node that is a success or running. 
             */
            if (childNodeState == NodeState.SUCCESS)
            {
                nodeState = NodeState.SUCCESS;
                return nodeState;
            }

            if (childNodeState == NodeState.RUNNING)
            {
                nodeState = NodeState.RUNNING;
                return nodeState;
            }
        }

        /*
         * If reached the end of the loop and none of the child nodes
         * are running or successful, then we know that the node state is
         * a failure.
         */
        nodeState = NodeState.FAILURE;
        return nodeState;
    }
}
