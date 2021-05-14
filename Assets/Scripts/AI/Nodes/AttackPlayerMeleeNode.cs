using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerMeleeNode : Node
{
    private EnemyVisionSphere visionSphere;
    private Transform agentTransform;
    private NavMeshAgent agent;
    private Animator anim;
    private float attackDistance;
    private float attackMovementSpeed;
    private bool attackStarted;
    private string meleeAttackString;
    private Vector3 playerPosition;

    public AttackPlayerMeleeNode(EnemyVisionSphere visionSphere, Transform agentTransform, NavMeshAgent agent, Animator anim, float attackDistance, float attackMovementSpeed)
    {
        this.visionSphere = visionSphere;
        this.agentTransform = agentTransform;
        this.agent = agent;
        this.anim = anim;
        this.attackDistance = attackDistance;
        this.attackMovementSpeed = attackMovementSpeed;
        playerPosition = visionSphere.PlayerPosition();
        attackStarted = false;
        meleeAttackString = "";
        nodeState = NodeState.NULL;
    }

    public override NodeState Evaluate()
    {

        //bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack");
        //if (nodeState == NodeState.RUNNING && !isAttacking)
        //{
        //    nodeState = NodeState.SUCCESS;
        //}
        //else if (nodeState == NodeState.RUNNING && isAttacking)
        //{
        //    agent.SetDestination(agentTransform.position + agentTransform.forward * 0.15f);
        //}
        //else if (nodeState != NodeState.RUNNING)
        //{
        //    agent.speed = 0.05f;
        //    anim.SetTrigger("enter_attack");
        //    nodeState = NodeState.RUNNING;
        //}
        playerPosition = visionSphere.PlayerPosition();

        if (nodeState != NodeState.RUNNING && Vector3.Distance(agentTransform.position, playerPosition) < attackDistance)
        {
            nodeState = NodeState.RUNNING;
            int randomAttackInt = Random.Range(1, 4);
            meleeAttackString = "enter_attack0" + randomAttackInt.ToString();
            anim.SetTrigger(meleeAttackString);
            agent.speed = attackMovementSpeed;
            agent.SetDestination(agentTransform.position + agentTransform.forward * 10);
        }

        if (Attacking())
        {
            attackStarted = true;
        }

        if (attackStarted && !Attacking())
        {
            attackStarted = false;
            nodeState = NodeState.FAILURE;
        }
        return nodeState;
    }

    private bool Attacking()
    {
        bool attacking = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack01") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack02") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack03"))
        {
            attacking = true;
        }

        return attacking;
    }
}
