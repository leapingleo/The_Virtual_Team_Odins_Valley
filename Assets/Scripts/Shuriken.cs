using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GrabThrow
{
    public float rotationSpeed;
    private Quaternion initialRotation;
    private bool collided;
    public float timeTillRespawn = 5f;
    private float timer;
    private Transform initialParent;
    private Vector3 initialPosition;
    public bool rotate;

    private void Start()
    {
        initialPosition = transform.position;
        initialParent = gameObject.transform.parent.transform;
        initialRotation = transform.rotation;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (grabbed)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                Respawn();
            }
        }

        if (rotate)
        {
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void Respawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.parent = initialParent;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        timer = timeTillRespawn;
        ResetAttributes();
    }

    public override void MoveToHandInit()
    {
        rotate = true;
        transform.parent = null;
        rb.isKinematic = false;
        transform.rotation = initialRotation;
        base.MoveToHandInit();
    }

    public override void MoveAwayHandInit()
    {
        transform.rotation = initialRotation;
        base.MoveAwayHandInit();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (rotate && collision.gameObject.CompareTag("ShurikenWall"))
        {
            transform.parent = collision.gameObject.transform;
            initialParent = transform.parent;
            initialPosition = transform.position;
            rotate = false;
            collided = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            ResetAttributes();
        }
        else if (rotate && !collision.gameObject.CompareTag("ShurikenWall"))
        {
            rotate = false;
            Respawn();
        }
        
    }

}
