using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    /* Current state of the node */
    protected NodeState nodeState;

    public NodeState NodeState
    {
        get { return nodeState; }
    }

    /* Implementing classes use this method to evaluate the desired set of conditions */
    public abstract NodeState Evaluate();

}
