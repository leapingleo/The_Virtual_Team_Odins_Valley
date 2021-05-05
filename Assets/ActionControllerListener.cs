using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControllerListener : MonoBehaviour
{
    public static ActionControllerListener Instance;
    public ActionController left;
    public ActionController right;

    private bool mainButtonPressed;
    public bool MainButtonPressed { get { return mainButtonPressed; } }

    private bool mainButtonReleased;
    public bool MainButtonReleased { get { return mainButtonReleased; } }

    private bool secondaryButtonDown;
    public bool SecondaryButtonDown { get { return secondaryButtonDown; } }
    private bool secondaryButtonPressed;
    public bool SecondaryButtonPressed { get { return secondaryButtonPressed; } }

    private bool triggerButtonPressed;
    public bool TriggerButtonPressed { get { return triggerButtonPressed; } }
    private Vector3 handPos;
    public Vector3 HandPos { get { return handPos; } }

    private Vector3 joystickDirection;
    public Vector3 JoystickDirection { get { return joystickDirection; } }


    void Awake()
    {
         if (Instance == null)
             Instance = this;
         else
             Destroy(gameObject);

        //  handObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    void Update()
    {
        HandListener();
    }

    void HandListener()
    {
        if (left.HandPos.magnitude > 0)
            handPos = left.HandPos;

        if (right.HandPos.magnitude > 0)
            handPos = right.HandPos;
    }

    void TriggerListener()
    {
        if (left.TriggerButtonPressed)
            triggerButtonPressed = left.TriggerButtonPressed;
    }
}
