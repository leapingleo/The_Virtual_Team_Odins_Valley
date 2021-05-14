using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxeNode : Node
{
    private EnemyVisionSphere vision;
    private EnemyAI enemyAI;
    private float axeWaitingPeriod;
    private float timer;

    public ThrowAxeNode(EnemyVisionSphere vision, EnemyAI enemyAI, float axeWaitingPeriod)
    {
        this.vision = vision;
        this.enemyAI = enemyAI;
        this.axeWaitingPeriod = axeWaitingPeriod;
        timer = -1;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }

}
