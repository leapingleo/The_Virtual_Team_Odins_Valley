using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnShield : MonoBehaviour
{
    public BoxCollider bc;
    public Rigidbody rb;
    public float respawnTime;
    private float timer = 0f;
    private bool activated = false;

    public void TurnOffShield()
    {
        bc.enabled = false;
        rb.isKinematic = true;
    }


    public void ActivateShield()
    {
        rb.isKinematic = false;
        bc.enabled = true;
        timer = respawnTime;
        activated = true;
        transform.parent = null;
    }

    public void Update()
    {
        if (activated)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
