using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public Transform toTeleport;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = toTeleport.position + toTeleport.forward * 0.10f;
            collision.gameObject.transform.rotation = toTeleport.rotation;
        }
    }

}   
    
