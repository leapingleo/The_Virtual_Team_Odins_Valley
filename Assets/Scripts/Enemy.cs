using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GrabThrow
{
    public int numHits;
    public float idleTime;
    public float updatePlayerPathTime;
    public float walkRadius;
    public EnemyVisionSphere visionSphere;
    public NavMeshAgent navmeshAgent;
    public Animator animator;
    public GameObject axe;

    private Vector3 walkDirection;
    private float timer;
    private EnemyState state;
    private float standardMovementSpeed;

    public enum EnemyState { IDLE, WALK, GRABBED, PLAYER_DETECTED, ATTACKING_PLAYER, DIZZY };

    // Start is called before the first frame update
    void Start()
    {
        canBeGrabThrown = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        timer = idleTime;
        standardMovementSpeed = navmeshAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != EnemyState.PLAYER_DETECTED && state != EnemyState.GRABBED && state != EnemyState.DIZZY)
        {
            if (visionSphere.PlayerDetected())
            {
                navmeshAgent.isStopped = false;
                timer = updatePlayerPathTime;
                animator.SetTrigger("enter_jog");
                state = EnemyState.PLAYER_DETECTED;
            }

        }

        if (state == EnemyState.IDLE)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                navmeshAgent.isStopped = false;
                state = EnemyState.WALK;
                animator.SetTrigger("enter_jog");
                Walk();
            }
        }

        if (state == EnemyState.WALK)
        {
            if (Vector3.Distance(transform.position, walkDirection) < 0.05)
            {
                navmeshAgent.isStopped = true;
                state = EnemyState.IDLE;
                timer = idleTime;
                animator.SetTrigger("enter_idle");
            }
        }

        if (state == EnemyState.GRABBED)
        {

        }

        if (state == EnemyState.PLAYER_DETECTED)
        {
            timer -= Time.deltaTime;
            Vector3 playerPosition = visionSphere.PlayerPosition();
            if (playerPosition == Vector3.zero)
            {
                timer = idleTime;
                state = EnemyState.IDLE;
                navmeshAgent.isStopped = true;
                animator.SetTrigger("enter_idle");
            }
            else if (timer < 0 && playerPosition != Vector3.zero)
            {
                timer = updatePlayerPathTime;
                navmeshAgent.destination = GetNavmeshPlayerPosition(playerPosition);
            }

            if (playerPosition != Vector3.zero && Vector3.Distance(transform.position, playerPosition) < 0.05f)
            {
                animator.SetTrigger("enter_attack");
            }

        }

        if (state == EnemyState.DIZZY)
        {

        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("enemy_hit"))
        {
            navmeshAgent.speed = 0f;
        }
        else
        {
            navmeshAgent.speed = standardMovementSpeed;
        }


    }

    public override void MoveToHandInit()
    {
        navmeshAgent.enabled = false;
        rb.isKinematic = false;
        state = EnemyState.GRABBED;
        animator.SetTrigger("enter_falling");
        rb.detectCollisions = true;
        axe.SetActive(false);
        base.MoveToHandInit();
    }

    public void GetHitByAxe()
    {
        GetHit(1);
    }

    private void GetHit(int damage)
    {
        animator.SetTrigger("enter_hit");
        numHits -= damage;

        if (numHits <= 0)
        {
            animator.SetTrigger("enter_dizzy");
            navmeshAgent.isStopped = true;
            canBeGrabThrown = true;
            state = EnemyState.DIZZY;
        }
    }

    void Walk()
    {
        walkDirection = GetRandomDirection();
        navmeshAgent.destination = walkDirection;
    }

    Vector3 GetRandomDirection()
    {
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        return hit.position;
    }

    Vector3 GetNavmeshPlayerPosition(Vector3 playerPosition)
    {
        Vector3 navmeshPlayerPosition = Vector3.zero;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(playerPosition, out hit, walkRadius, 1))
        {
            navmeshPlayerPosition = hit.position;
        }
        return navmeshPlayerPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && collision.gameObject.GetComponent<GrabThrow>().grabbed)
        {
            GetHit(1);
        }

        if (grabbed)
        {
            gameObject.SetActive(false);
        }
    }

    public void TurnOnAxeCollision()
    {
        axe.GetComponent<AxeCollision>().TurnOnCollider();
    }

    public void TurnOffAxeCollision()
    {
        axe.GetComponent<AxeCollision>().TurnOffCollider();
    }
}
