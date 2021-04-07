using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionSphere : MonoBehaviour
{
    private bool detectedPlayer = false;
    private Vector3 playerPosition = Vector3.zero;

    public bool PlayerDetected()
    {
        return detectedPlayer;
    }

    public Vector3 PlayerPosition()
    {
        return playerPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectedPlayer = true;
            playerPosition = other.gameObject.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectedPlayer = true;
            playerPosition = other.gameObject.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectedPlayer = false;
            playerPosition = Vector3.zero;
        }
    }


}
