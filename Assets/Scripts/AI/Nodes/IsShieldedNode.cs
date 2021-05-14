using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsShieldedNode : Node
{
    private EnemyAI enemyAI;
    private Animator anim;

    public IsShieldedNode(EnemyAI enemyAI, Animator anim)
    {
        this.enemyAI = enemyAI;
        this.anim = anim;
    }

    public override NodeState Evaluate()
    {
        if (enemyAI.hasShield || (enemyAI.hasShield && HitByProjectileAnimPlaying()))
        {
            nodeState = NodeState.SUCCESS;
        }
        else
        {
            nodeState = NodeState.FAILURE;
        }

        return nodeState;
    }

    private bool HitByProjectileAnimPlaying()
    {
        bool animPlaying = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_projectile01") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_projectile02") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_projectile03"))
        {
            animPlaying = true;
        }

        return animPlaying;
    }


}
