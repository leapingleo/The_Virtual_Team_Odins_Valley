using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHitNode : Node
{
    private EnemyAI enemyAI;
    private Animator anim;
    private NavMeshAgent agent;
    private float initialSpeed;
    private bool hitAxeAnimStarted;
    private bool hitProjectileAnimStarted;
    private string hitAnimString;
    
    public GetHitNode(EnemyAI enemyAI, Animator anim, NavMeshAgent agent)
    {
        this.enemyAI = enemyAI;
        this.anim = anim;
        this.agent = agent;
        nodeState = NodeState.FAILURE;
    }

    public override NodeState Evaluate()
    {
        if (enemyAI.hitByAxe)
        {
            enemyAI.hitByProjectile = false;
            enemyAI.hitByAxe = false;
            int randomAttackInt = Random.Range(1, 3);
            hitAnimString = "enter_hit_axe0" + randomAttackInt.ToString();
            anim.SetTrigger(hitAnimString);
            nodeState = NodeState.RUNNING;
            agent.isStopped = true;
        }

        if (enemyAI.hitByProjectile)
        {
            enemyAI.hitByProjectile = false;
            enemyAI.hitByAxe = false;
            int randomAttackInt = Random.Range(1, 3);
            hitAnimString = "enter_hit_projectile0" + randomAttackInt.ToString();
            anim.SetTrigger(hitAnimString);
            nodeState = NodeState.RUNNING;
            agent.isStopped = true;
        }

        if (HitByAxeAnimPlaying())
        {
            hitAxeAnimStarted = true;
        }

        if (HitByProjectileAnimPlaying())
        {
            hitProjectileAnimStarted = true;
        }

        if ((hitAxeAnimStarted && !HitByAxeAnimPlaying()) || (hitProjectileAnimStarted && !HitByProjectileAnimPlaying()))
        {
            hitProjectileAnimStarted = false;
            hitAxeAnimStarted = false;
            agent.isStopped = false;
            nodeState = NodeState.SUCCESS;
        }

        if (nodeState != NodeState.RUNNING || nodeState != NodeState.SUCCESS)
        {
            nodeState = NodeState.FAILURE;
        }

        

        return nodeState;


    }

    private bool HitByAxeAnimPlaying()
    {
        bool animPlaying = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_axe01") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit_axe02"))
        {
            animPlaying = true;
        }

        return animPlaying;
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
