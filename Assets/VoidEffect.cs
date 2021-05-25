using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject, 1f);
        }
        
    }

    
}
