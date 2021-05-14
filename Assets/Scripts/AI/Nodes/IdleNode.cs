using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    Animator anim;

    public IdleNode(Animator anim)
    {
        this.anim = anim;
    }

    public override NodeState Evaluate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_idle"))
        {
            anim.SetTrigger("enter_idle");
        }
        else
        {
            nodeState = NodeState.RUNNING;
        }

        return nodeState;
    }
}
