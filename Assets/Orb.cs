using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float rotationSpeed;

    private GameObject door;
    private bool animateOrb = false;
    private Transform child;

    private void Start()
    {
        child = transform.GetChild(0);   
    }

    public void SetDoor(GameObject door)
    {
        this.door = door;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!animateOrb && collision.gameObject.layer == 9 && collision.gameObject.GetComponent<GrabThrow>().grabbed)
        {
            door.GetComponent<Door>().OrbActivated();
            animateOrb = true;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!animateOrb && other.CompareTag("Axe"))
    //    {
    //        door.GetComponent<Door>().OrbActivated();
    //    }
    //}

    public void GetHitByAxe()
    {
        if (!animateOrb)
        {
            door.GetComponent<Door>().OrbActivated();
            animateOrb = true;
        }
    }

    private void Update()
    {
        if (animateOrb)
        {
            child.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
    }

}
