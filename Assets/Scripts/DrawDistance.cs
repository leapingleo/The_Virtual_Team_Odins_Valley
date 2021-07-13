using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDistance : MonoBehaviour
{

    private Transform character;
    private Transform VRcharacter;

    private Transform center;
    public bool disableGeometry = false;
    public float distanceTillDraw = 1f;
    public float distanceTillDrawGeometry = 3f;
    private bool disabled = false;
    private bool disabledGeometry = false;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;
        VRcharacter = GameObject.FindGameObjectWithTag("XR").transform;
        center = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        return;
        float distance = Vector3.Distance(character.transform.position, center.transform.position);
       // Debug.Log(distance);
        if (!disabled && distance > distanceTillDraw)
        {
            Debug.Log(true);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            disabled = true;
        }
        else if (disabled && distance <= distanceTillDraw)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            disabled = false;
        }

        if (disableGeometry)
        {
            float VRdistance = Vector3.Distance(VRcharacter.transform.position, center.transform.position);
            if (!disabledGeometry && VRdistance > distanceTillDrawGeometry)
            {
                Debug.Log(true);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                disabledGeometry = true;
            }
            else if (disabledGeometry && VRdistance <= distanceTillDrawGeometry)
            {
                transform.GetChild(4).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(true);
                transform.GetChild(6).gameObject.SetActive(true);
                disabledGeometry = false;
            }
        }


    }
}
