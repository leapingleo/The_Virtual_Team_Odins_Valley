using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSightNode : Node
{
    EnemyVisionSphere visionSphere;
    Animator anim;

    public PlayerInSightNode(EnemyVisionSphere visionSphere, Animator anim)
    {
        this.visionSphere = visionSphere;
        this.anim = anim;
    }

    public override NodeState Evaluate()
    {
        /*
        * This is definately a lazy fix, but with the shield enemy it will finish its shield animation and will 
        * look while falling which looks weird, so lazily fixed with this solution. 
        */

        if (visionSphere.PlayerDetected() && !HitByProjectileAnimPlaying())
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
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_projectile02"))
        {
            animPlaying = true;
        }

        return animPlaying;
    }
}
