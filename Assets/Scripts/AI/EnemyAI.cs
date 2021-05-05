using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : GrabThrow
{
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyVisionSphere visionSphere;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float distanceTillAttack;
    //[SerializeField] private float attackDistance;
    private float movementSpeed;
    private Selector root;

    private void Start()
    {
        movementSpeed = agent.speed;
        ConstructBehaviourTree();
        rb.isKinematic = true;
    }

    /*
     * When constructing a behaviour tree, you must use bottom up approach.
     */
    private void ConstructBehaviourTree()
    {
        PlayerInSightNode playerInSightNode = new PlayerInSightNode(visionSphere);
        AttackPlayerMeleeNode attackPlayerMeleeNode = new AttackPlayerMeleeNode(visionSphere, transform, agent, anim, distanceTillAttack, attackSpeed);
        MoveToPlayerNode moveToPlayerNode = new MoveToPlayerNode(0f, visionSphere, agent, transform, anim, movementSpeed);

        Selector meleeAttackSelector = new Selector(new List<Node> { attackPlayerMeleeNode, moveToPlayerNode });

        Sequence inSightSequence = new Sequence(new List<Node> { playerInSightNode, meleeAttackSelector });

        IdleNode idleNode = new IdleNode(anim);

        root = new Selector(new List<Node> { inSightSequence, idleNode });
    }

    private void Update()
    {
        root.Evaluate();
    }




}
