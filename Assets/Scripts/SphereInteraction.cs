using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public bool appeared;
    public Collider collider;
    public GameObject sphere;
    private float detectRadius = 0.8f;

    // Start is called before the first frame update
    void Awake()
    {
        if (appeared)
            Appear();
        else
            Disappear();
        
    }

    // Update is called once per frame
    void Update()
    {
        float sqrtD = Vector3.SqrMagnitude(transform.position - sphere.transform.position);

        if (sqrtD < detectRadius * detectRadius)
        {
            Appear();
        } else
        {
            Disappear();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag.Equals("sphere"))
    //    {
    //        if (appeared)
    //            Disappear();
    //        else
    //            Appear();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag.Equals("sphere"))
    //    {
    //        if (appeared)
    //            Disappear();
    //        else
    //            Appear();
    //    }
    //}

    void Appear()
    {
        collider.isTrigger = false;
        appeared = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    void Disappear()
    {
        collider.isTrigger = true;
        appeared = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

}
