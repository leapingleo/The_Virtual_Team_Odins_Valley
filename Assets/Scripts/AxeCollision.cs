using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{

    public BoxCollider collider;
    public bool playerAxe;
    public GameObject burstParticle;

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
                EnemyAI enemyScript = other.gameObject.GetComponent<EnemyAI>();
                if (!enemyScript.hasShield)
                {
                    GameObject particle = Instantiate(burstParticle, transform.position, Quaternion.identity);
                    Destroy(particle, 0.5f);
                }
                Debug.Log("enemy");
                if (enemyScript)
                {
                    other.gameObject.GetComponent<EnemyAI>().GetHitByAxe();
                   
                }
            }

            if (other.CompareTag("Crate"))
            {
                other.gameObject.GetComponent<Crate>().GetHitByAxe();
            }

            if (other.CompareTag("DoorOrb"))
            {
                other.gameObject.GetComponent<Orb>().GetHitByAxe();
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<CharacterMovement>().GetHitByEnemyAxe();
            }
        }

    }

}
