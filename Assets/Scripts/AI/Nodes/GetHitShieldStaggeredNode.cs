using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHitShieldStaggeredNode : Node
{
    private EnemyAI enemyAI;
    private Animator anim;
    private NavMeshAgent agent;
    private Transform enemyTransform;
    private EnemyVisionSphere visionSphere;
    private bool hitAnimStarted;
    private string hitAnimString;

    public GetHitShieldStaggeredNode(EnemyAI enemyAI, Animator anim, NavMeshAgent agent, Transform enemyTransform, EnemyVisionSphere visionSphere)
    {
        this.enemyAI = enemyAI;
        this.anim = anim;
        this.agent = agent;
        this.enemyTransform = enemyTransform;
        this.visionSphere = visionSphere;
        nodeState = NodeState.FAILURE;
    }

    public override NodeState Evaluate()
    {
        return NodeState.NULL;
    }
}
