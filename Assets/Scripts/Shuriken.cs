using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GrabThrow
{
    [Header("Shuriken Attributes")]
    public float rotationSpeed;
    public float timeTillRespawn = 0.45f;
    private Transform initialParent;
    public bool rotate = false;

    private void Start()
    {
        initialPosition = transform.position;
        initialParent = gameObject.transform.parent.transform;
        initialRotation = transform.rotation;
        timer = timeTillRespawn;
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
        rotate = false;
        rb.velocity = Vector3.zero;
        bc.enabled = true;
        rb.isKinematic = true;
        grabbed = false;
        timer = timeTillRespawn;

    }

    public override void MoveToHandInit()
    {
        rotate = true;
        transform.parent = null;
        rb.isKinematic = false;
        base.MoveToHandInit();
    }

    public override void MoveToReleasePositionInit()
    {
        base.MoveToReleasePositionInit();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (grabbed && collision.gameObject.CompareTag("ShurikenWall"))
        {
            transform.rotation = initialRotation;
            transform.parent = collision.gameObject.transform;
            initialParent = transform.parent;
            initialPosition = transform.position;
            rotate = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            grabbed = false;
            timer = timeTillRespawn;
        }
        else if (grabbed && !collision.gameObject.CompareTag("ShurikenWall"))
        {
            Respawn();
            
        }

    }

}
