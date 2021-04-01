using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GrabThrow
{

    public float idleTime;
    public float walkRadius;
    public NavMeshAgent navmeshAgent;
    public Animator animator;

    private Vector3 walkDirection;
    private float timer;
    private EnemyState state;

    public enum EnemyState { IDLE, WALK, GRABBED };

    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = false;
        timer = idleTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.IDLE)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                state = EnemyState.WALK;
                animator.SetTrigger("enter_jog");
                Walk();
            }
        }

        if (state == EnemyState.WALK)
        {
            if (Vector3.Distance(transform.position, walkDirection) < 0.05)
            {
                state = EnemyState.IDLE;
                timer = idleTime;
                animator.SetTrigger("enter_idle");
            }
        }

        if (state == EnemyState.GRABBED)
        {

        }

    }

    public override void MoveToHandInit()
    {
        navmeshAgent.enabled = false;
        state = EnemyState.GRABBED;
        animator.SetTrigger("enter_falling");
        rb.detectCollisions = true;
        base.MoveToHandInit();

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
}
