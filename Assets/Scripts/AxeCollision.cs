using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{

    public BoxCollider collider;
    public bool playerAxe;
    void Start()
    {
        collider.enabled = false;
    }

    public void TurnOnCollider()
    {
        collider.enabled = true;
    }

    public void TurnOffCollider()
    {
        collider.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (playerAxe)
        {
            if (other.CompareTag("Throwable"))
            {
                Enemy enemyScript = other.gameObject.GetComponent<Enemy>();

                if (enemyScript)
                {
                    other.gameObject.GetComponent<Enemy>().GetHitByAxe();
                }
            }
        }
        else
        {

        }
    }

}
