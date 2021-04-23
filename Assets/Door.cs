using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float detectRadius = 50;
    private int numberOfOrbs = 0;
    private int numberOfActiveOrbs = 0;
    private bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, detectRadius);
       
        foreach (Collider col in objects)
        {
            if (col.CompareTag("DoorOrb"))
            {
                col.gameObject.GetComponent<Orb>().SetDoor(gameObject);
                numberOfOrbs += 1;
            }
        }
    }

    public void OrbActivated()
    {
        numberOfActiveOrbs += 1;

        if (!opened && numberOfActiveOrbs == numberOfOrbs)
        {
            OpenDoor();
            opened = true;
        }
    }

    private void OpenDoor()
    {
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("open");
        transform.GetChild(1).GetComponent<Animator>().SetTrigger("open");
    }
}
