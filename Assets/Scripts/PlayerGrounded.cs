using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    public bool grounded;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 9 || other.gameObject.layer != 10)
            grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9 || other.gameObject.layer != 10)
            grounded = false;
    }


}
