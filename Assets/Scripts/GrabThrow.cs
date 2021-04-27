using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrow : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider bc;
    public float moveToSpeed;
    public float moveAwaySpeed;
    public float respawnTime;
    public float angularVelocityMultiplier;
    
    public bool canBeGrabThrown = true;
    public bool respawn = false;
    public bool grabbed = false;
    private bool moveToHandInit = true;
    private bool moveAwayHandInit = true;
    private bool movedToHand = false;
    public bool collided = false;
    private Transform initialTransform;

    private void Start()
    {
        initialTransform = transform;
    }

    public void ResetAttributes()
    {
        grabbed = false;
        canBeGrabThrown = true;
        moveToHandInit = true;
        moveAwayHandInit = true;
        movedToHand = false;
        collided = false;
    }


    public void MoveToHand(Transform controller)
    {
        if (moveToHandInit)
        {
            MoveToHandInit();
            moveToHandInit = false;
        }

        if (!movedToHand)
        {
            WhileMoving();

            float distance = Vector3.Distance(transform.position, controller.position);

            if (distance < 0.1)
            {
                MovedToHandInit(controller);
                movedToHand = true;
            }
            else
            {
                rb.velocity = (controller.position - transform.position).normalized * moveToSpeed * Time.fixedDeltaTime;
            }
        }
    }

    public void MoveToReleasePosition(Vector3 point)
    {
        if (!collided)
        {
            if (moveAwayHandInit)
            {
                MoveAwayHandInit();
                moveAwayHandInit = false;
            }

            WhileMoving();
            rb.velocity = (point - transform.position).normalized * moveAwaySpeed * Time.fixedDeltaTime;
        }
    }

    public virtual void MovedToHandInit(Transform controller)
    {
        rb.velocity = Vector3.zero;
        transform.position = controller.position;
        transform.rotation = controller.rotation;
        transform.parent = controller;
    }

    public virtual void MoveToHandInit()
    {
        rb.angularVelocity = transform.up * angularVelocityMultiplier * Time.fixedDeltaTime;
        bc.enabled = false;
        rb.useGravity = false;
    }

    public virtual void MoveAwayHandInit()
    {
        grabbed = true;
        transform.parent = null;
        bc.enabled = true;
    }

    public virtual void WhileMoving()
    {

    }

    public virtual void Collision(Collision collision)
    {
        gameObject.SetActive(false);

        //if (respawn)
        //{
        //    transform.GetChild(0).gameObject.SetActive(false);
        //    //StartCoroutine(Respawn(respawnTime));
        //    BecomeRespawned();
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public virtual void BecomeRespawned()
    {
        canBeGrabThrown = true;
        grabbed = false;
        moveToHandInit = true;
        moveAwayHandInit = true;
        movedToHand = false;
        bc.enabled = true;
        rb.useGravity = true;
        rb.angularVelocity = Vector3.zero;
        collided = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (grabbed)
        {
            collided = true;
            gameObject.SetActive(false);
        }
        
    }

    IEnumerator Respawn(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        BecomeRespawned();    
    }


}
