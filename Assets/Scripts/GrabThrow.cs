using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrow : MonoBehaviour
{
    [Header("Grab Throw Attributes")]
    public Rigidbody rb;
    public BoxCollider bc;
    public float moveToSpeed;
    public float moveAwaySpeed;
    public float angularVelocityMultiplier;
    
    public bool canBeGrabThrown = true;
    public bool grabbed = false;
    protected bool movedToHand = false;
    public bool collided = false;
    protected bool movedToReleasePosition = false;
    public bool respawn = false;
    protected Vector3 initialPosition = Vector3.zero;
    protected Quaternion initialRotation = Quaternion.identity;
    private TrailRenderer trailRenderer = null;
    public GameObject explosionPrefab;
    protected float timer = -1;
    private float respawnTime = 10f;
    


    private void Start()
    {
        if (respawn)
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            trailRenderer = GetComponent<TrailRenderer>();
        }
            
    }

    private void Update()
    {
        /*
        * This essentially respawns something after period of time
        */

        if (timer > -1)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                grabbed = false;
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                bc.enabled = true;
                rb.useGravity = true;
                trailRenderer.enabled = true;
                transform.GetChild(0).gameObject.SetActive(true);
                timer = -1;
            }
        }
    }


    public void MoveToHand(Transform controller)
    {
        if (!movedToHand)
        {
            float distance = Vector3.Distance(transform.position, controller.position);

            if (distance < 0.1)
            {
                rb.velocity = Vector3.zero;
                transform.position = controller.position;
                transform.rotation = controller.rotation;
                transform.parent = controller;
            }
            else
            {
                rb.velocity = (controller.position - transform.position).normalized * moveToSpeed * Time.fixedDeltaTime;
            }
        }
    }

    public void MoveToReleasePosition(Vector3 point)
    {
        if (!movedToReleasePosition)
        {
            rb.velocity = (point - transform.position).normalized * moveAwaySpeed * Time.fixedDeltaTime;
        }
    }

    public void CallMoveToHandInit()
    {
        MoveToHandInit();
    }

    /*
     * METHOD IS CALLED BY OBJECT DETECTOR
     */
    public virtual void MoveToHandInit()
    {
        movedToHand = false;
        rb.angularVelocity = transform.up * angularVelocityMultiplier * Time.fixedDeltaTime;
        bc.enabled = false;
        rb.useGravity = false;
    }

    public void CallMoveToReleasePositionInit()
    {
        MoveToReleasePositionInit();
    }

    /*
     * METHOD IS CALLED BY OBJECT DETECTOR
     */
    public virtual void MoveToReleasePositionInit()
    {
        movedToHand = true;
        movedToReleasePosition = false;
        grabbed = true;
        transform.parent = null;
        bc.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (grabbed)
        {
            movedToReleasePosition = true;
            if (respawn)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                bc.enabled = false;
                rb.useGravity = false;
                trailRenderer.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                timer = respawnTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
            Quaternion alignToNormal = Quaternion.FromToRotation(explosionPrefab.transform.forward, collision.contacts[0].normal);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, alignToNormal);
            Destroy(explosion, 1.5f);
        }

        
    }


}
