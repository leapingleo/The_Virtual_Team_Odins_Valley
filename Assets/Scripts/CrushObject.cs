using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "crusher")
            Destroy(gameObject);
    }
}
