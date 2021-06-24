using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAimReticle : MonoBehaviour
{

    public bool turnedOff = false;
    public bool hand = false;
    public bool reticle = false;

    private void Start()
    {
        TurnOff();

        
    }

    public void TurnOff()
    {
        turnedOff = true;
        hand = false;
        reticle = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void TurnOnHand()
    {
        turnedOff = false;
        hand = true;
        reticle = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void TurnOnReticle()
    {
        turnedOff = false;
        hand = false;
        reticle = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
