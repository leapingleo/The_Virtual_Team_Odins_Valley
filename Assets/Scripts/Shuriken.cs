using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GrabThrow
{
    public float rotationSpeed;
    private Quaternion initialRotation;
    private bool collided;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public override void MoveToHandInit()
    {
        transform.rotation = initialRotation;
        base.MoveToHandInit();
    }

    public override void MoveAwayHandInit()
    {
        transform.rotation = initialRotation;
        base.MoveAwayHandInit();
    }

    public override void WhileMoving()
    {
        if (!collided)
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
    }

    public override void Collision(Collision collision)
    {
        if (!collided && collision.gameObject.CompareTag("ShurikenWall"))
        {
            collided = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
