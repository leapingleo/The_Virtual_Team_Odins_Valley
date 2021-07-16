using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  Debug.Log("y " + other.GetComponent<CharacterMovement>().playerNormal.GroundNormal.y);
            if (!other.GetComponent<CharacterMovement>().Grounded && other.GetComponent<CharacterMovement>().inGravityLevel
                && other.GetComponent<CharacterMovement>().playerNormal.GroundNormal.y < 0.98f)
            {
                other.GetComponent<CharacterMovement>().inGravityLevel = false;
                other.GetComponent<CharacterMovement>().Respawn();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if (!other.GetComponent<CharacterMovement>().Grounded)
            //    other.GetComponent<CharacterMovement>().Respawn();
            other.GetComponent<CharacterMovement>().inGravityLevel = true;
        }
    }
}
