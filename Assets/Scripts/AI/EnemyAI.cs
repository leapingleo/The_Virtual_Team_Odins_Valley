using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : GrabThrow
{
    [Header("EnemyAI")]
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyVisionSphere visionSphere;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float distanceTillAttack;
    public bool hasShield;
    public GameObject shield;
    private float movementSpeed;
    private Selector root;
    public bool hitByAxe;
    public bool hitByProjectile;
    public Vector3 hitPosition;
    private int numHits;
    private bool isDizzy = false;
    public GameObject axe;
    public AxeCollision axeCollision;

    private void Start()
    {
        numHits = Random.Range(4, 10);
        movementSpeed = agent.speed;
        ConstructBehaviourTree();
        rb.isKinematic = true;
    }

    /*
     * When constructing a behaviour tree, you must use bottom up approach.
     */
    private void ConstructBehaviourTree()
    {

        PlayerInSightNode playerInSightNode = new PlayerInSightNode(visionSphere, anim);
        AttackPlayerMeleeNode attackPlayerMeleeNode = new AttackPlayerMeleeNode(visionSphere, transform, agent, anim, distanceTillAttack, attackSpeed);
        MoveToPlayerNode moveToPlayerNode = new MoveToPlayerNode(0f, visionSphere, agent, transform, anim, movementSpeed);
        GetHitNode getHitNode = new GetHitNode(this, anim, agent);
        
        Selector hitShieldSelector = null;


        if (hasShield)
        {
            GetHitShieldNode getHitShieldNode = new GetHitShieldNode(this, anim, agent, transform, visionSphere);
            IsShieldedNode isShieldedNode = new IsShieldedNode(this, anim);
            hitShieldSelector = new Selector(new List<Node> { new Sequence(new List<Node> { isShieldedNode, getHitShieldNode }), getHitNode });
        }
        
        Selector meleeAttackSelector = new Selector(new List<Node> { attackPlayerMeleeNode, moveToPlayerNode });

        Sequence inSightSequence = new Sequence(new List<Node> { playerInSightNode, meleeAttackSelector });

        IdleNode idleNode = new IdleNode(anim);

        if (hasShield)
            root = new Selector(new List<Node> { hitShieldSelector, inSightSequence, idleNode });
        else
            root = new Selector(new List<Node> { getHitNode, inSightSequence, idleNode });
    }

    private void Update()
    {
        if (!canBeGrabThrown)
            root.Evaluate();
    }

    void TakeDamage()
    {
        numHits -= 1;

        if (numHits < 0)
        {
            canBeGrabThrown = true;
            anim.SetTrigger("enter_dizzy");
        }
    }

    public void GetHitByAxe()
    {
        
        hitByAxe = true;

        if (!hasShield)
            TakeDamage();
    }

    public override void MoveToHandInit()
    {
        rb.isKinematic = false;
        agent.enabled = false;
        anim.SetTrigger("enter_falling");
        axe.gameObject.SetActive(false);
        base.MoveToHandInit();
    }

    public void TurnOnAxeCollision()
    {
        axeCollision.TurnOnCollider();
    }

    public void TurnOffAxeCollision()
    {
        axeCollision.TurnOffCollider();
    }

    public void TurnOnShield()
    {
        if (hasShield)
        {
            shield.GetComponent<TurnOnShield>().ActivateShield();
            hasShield = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && collision.gameObject.GetComponent<GrabThrow>().grabbed)
        {

            hitByProjectile = true;
            TakeDamage();
        }

        if (grabbed)
        {
            gameObject.SetActive(false);
        }
    }

}
