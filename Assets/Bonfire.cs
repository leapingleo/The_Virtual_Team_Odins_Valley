using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private int checkPointID;
    public int CheckPointID { get { return checkPointID; } set { checkPointID = value; } }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            other.GetComponent<CharacterMovement>().CheckPointPos = transform.position + Vector3.up * 0.5f;
        }
    }
}
