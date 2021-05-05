using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSightNode : Node
{
    EnemyVisionSphere visionSphere;

    public PlayerInSightNode(EnemyVisionSphere visionSphere)
    {
        this.visionSphere = visionSphere;
    }

    public override NodeState Evaluate()
    {
        if (visionSphere.PlayerDetected())
        {
            nodeState = NodeState.SUCCESS;
        }
        else
        {
            nodeState = NodeState.FAILURE;
        }

        return nodeState;
    }
}
