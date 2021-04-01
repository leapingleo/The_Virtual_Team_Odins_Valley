using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrow : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider bc;
    public float moveToSpeed;
    public float moveAwaySpeed;

    private bool moveToHandInit = true;
    private bool moveAwayHandInit = true;
    private bool movedToHand = false;


    private void FixedUpdate()
    {
        //if (releasePosition != Vector3.zero)
        //{
        //    MoveToReleasePosition(releasePosition);
        //    RaycastHit hit;
        //    if (Physics.Linecast(previousPosition, transform.position, out hit))
        //    {
        //        ObjectThrown();
        //    }
        //    else if (lerpHelper.Finished())
        //    {
        //        ObjectThrown();
        //    }

        //    previousPosition = transform.position;
        //}
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
        if (moveAwayHandInit)
        {
            MoveAwayHandInit();
            moveAwayHandInit = false;
        }

        WhileMoving();
        rb.velocity = (point - transform.position).normalized * moveAwaySpeed * Time.fixedDeltaTime;
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
        bc.enabled = false;
        rb.useGravity = false;
    }

    public virtual void MoveAwayHandInit()
    {
        transform.parent = null;
        bc.enabled = true;
    }

    public virtual void WhileMoving()
    {

    }

    public virtual void Collision(Collision collision)
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!moveAwayHandInit)
            Collision(collision);
    }




}
