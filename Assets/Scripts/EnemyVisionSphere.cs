using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionSphere : MonoBehaviour
{
    private bool detectedPlayer = false;
    private GameObject player = null;

    public bool PlayerDetected()
    {
        return detectedPlayer;
    }

    // Behaviour tree will check if player is detected,
    // so its okay if player has already been seen. 
    public Vector3 PlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!detectedPlayer)
            {
                detectedPlayer = true;
                player = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!detectedPlayer)
            {
                detectedPlayer = true;
                player = other.gameObject;
            }
        }
    }



}
