using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetHitShieldNode : Node
{
    private EnemyAI enemyAI;
    private Animator anim;
    private NavMeshAgent agent;
    private Transform enemyTransform;
    private EnemyVisionSphere visionSphere;
    private Vector3 playerPosition;
    private bool shieldHitAnimStarted;
    private bool projectileHitAnimStarted;
    private string hitAnimString;
    
    public GetHitShieldNode(EnemyAI enemyAI, Animator anim, NavMeshAgent agent, Transform enemyTransform, EnemyVisionSphere visionSphere)
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
        if (enemyAI.hitByAxe)
        {
            enemyAI.hitByAxe = false;
            enemyAI.hitByProjectile = false;
            anim.SetTrigger("enter_shield");
            nodeState = NodeState.RUNNING;
            Vector3 playerPosition = visionSphere.PlayerPosition();
            Vector3 toLookAt = new Vector3(playerPosition.x, enemyTransform.position.y, playerPosition.z);
            enemyTransform.LookAt(toLookAt);
            agent.isStopped = true;
        }

        if (enemyAI.hitByProjectile)
        {
            enemyAI.hitByProjectile = false;
            enemyAI.hitByAxe = false;
            int randomAttackInt = Random.Range(1, 3);
            hitAnimString = "enter_hit_projectile0" + randomAttackInt.ToString();
            anim.SetTrigger(hitAnimString);
            agent.isStopped = true;
            nodeState = NodeState.RUNNING;
        }

        if (ShieldAnimPlaying())
        {
            shieldHitAnimStarted = true;
        }

        if (HitByProjectileAnimPlaying())
        {
            projectileHitAnimStarted = true;
        }

        if ((shieldHitAnimStarted && !ShieldAnimPlaying()) || (projectileHitAnimStarted && !HitByProjectileAnimPlaying()))
        {
            projectileHitAnimStarted = false;
            shieldHitAnimStarted = false;
            agent.isStopped = false;
            nodeState = NodeState.SUCCESS;
        }

        if (nodeState != NodeState.RUNNING || nodeState != NodeState.SUCCESS)
        {
            nodeState = NodeState.FAILURE;
        }


        return nodeState;


    }

    private bool ShieldAnimPlaying()
    {
        bool animPlaying = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_shield"))
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
