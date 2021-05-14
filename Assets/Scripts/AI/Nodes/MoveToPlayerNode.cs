using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayerNode : Node
{
    private float updatePositionTimer;
    private EnemyVisionSphere visionSphere;
    private NavMeshAgent agent;
    private Transform enemyTransform;
    private Animator anim;
    private float movementSpeed;
    private Vector3 playerPosition;
    private float timer;

    public MoveToPlayerNode(float updatePositionTimer, EnemyVisionSphere visionSphere, NavMeshAgent agent, Transform enemyTransform, Animator anim, float movementSpeed)
    {
        this.updatePositionTimer = updatePositionTimer;
        this.visionSphere = visionSphere;
        this.agent = agent;
        this.enemyTransform = enemyTransform;
        this.anim = anim;
        this.movementSpeed = movementSpeed;
        
        playerPosition = this.visionSphere.PlayerPosition();
        timer = this.updatePositionTimer;
        //anim.SetTrigger("enter_jog");
    }

    public override NodeState Evaluate()
    {
        playerPosition = visionSphere.PlayerPosition();
        //if (Vector3.Distance(enemyTransform.position, playerPosition) < 0.045 || attackPlayerMeleeNode.NodeState == NodeState.RUNNING)
        //{
        //    nodeState = NodeState.SUCCESS;
        //}
        //else
        //{
        //    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_jog"))
        //        anim.SetTrigger("enter_jog");
        //    enemyTransform.LookAt(playerPosition);
        //    agent.SetDestination(playerPosition);
        //    nodeState = NodeState.RUNNING;
        //}
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_jog"))
            anim.SetTrigger("enter_jog");

        if (agent.speed != movementSpeed)
            agent.speed = movementSpeed;

        Vector3 toLookAt = new Vector3(playerPosition.x, enemyTransform.position.y, playerPosition.z);
        enemyTransform.LookAt(toLookAt);
        agent.SetDestination(playerPosition);
        nodeState = NodeState.RUNNING;

        return nodeState;
    }

    //void Update()
    //{
    //    timer -= Time.deltaTime;

    //    if (timer < 0)
    //    {
    //        playerPosition = visionSphere.PlayerPosition();
    //        timer = updatePositionTimer;
    //    }

    //    agent.SetDestination(playerPosition);
    //}
}
