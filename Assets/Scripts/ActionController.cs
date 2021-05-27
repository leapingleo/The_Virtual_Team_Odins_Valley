using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionController : MonoBehaviour
{
    public static ActionController Instance;
    public InputAction mainButton;
    public InputAction secondaryButton;
    public InputAction joystick;
    public InputAction leftTriggerButton;
    public InputAction rightTriggerButton;
    public InputAction leftControllerPos;
    public InputAction rightControllerPos;
    public InputAction leftGripButton;
    public InputAction rightGripButton;

    //public CharacterMovement character;
    public float gripPressedValue;
    private bool activationPressed;
    public bool ActivationPressed { get { return activationPressed; } }

    private bool mainButtonDown;
    public bool MainButtonDown { get { return mainButtonDown; } }

    private bool mainButtonPressed;
    public bool MainButtonPressed { get { return mainButtonPressed; } }

    private bool mainButtonReleased;
    public bool MainButtonReleased { get { return mainButtonReleased; } }

    private bool secondaryButtonDown;
    public bool SecondaryButtonDown { get { return secondaryButtonDown; } }

    private bool ignoreMainButtonRelease;
    private bool secondaryButtonPressed;
    public bool SecondaryButtonPressed { get { return secondaryButtonPressed; } }
    private bool selectPressed;

    private bool triggerButtonPressed;
    public bool TriggerButtonPressed { get { return triggerButtonPressed; } }

    private bool triggerButtonReleased;
    public bool TriggerButtonReleased { get { return triggerButtonReleased; } }

    private bool secondaryButtonReleased;
    public bool SecondaryButtonReleased { get { return secondaryButtonReleased; } }
    public bool SelectPressed { get { return selectPressed; } }
    private Vector3 leftHandPos;
    public Vector3 HandPos { get { return leftHandPos; } }

    public Vector3 LeftHandPos { get { return leftHandPos; } }
    private Vector3 rightHandPos;
    public Vector3 RightHandPos { get { return rightHandPos; } }

    private Vector3 joystickDirection;
    public Vector3 JoystickDirection { get { return joystickDirection; } }

    private bool triggerOccupied = false;
    public bool TriggerOccupied { set { triggerOccupied = value; } get { return triggerOccupied; } }

    private bool leftTriggerPressed;
    public bool LeftTriggerPressed { get { return leftTriggerPressed; } }
    private bool rightTriggerPressed;
    public bool RightTriggerPressed { get { return rightTriggerPressed; } }
    public float rightGripValue;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //  handObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

        // pushing down the joystick will activate this.
        // controller.uiPressAction.action.performed += UIActionPerformed;

        // controller.selectAction.action.performed += SelectionActionPerformed;
        //  controller.selectAction.action.canceled += SelectionActionCanceled;

        //controller.uiPressAction.action.performed += TriggerActionPerformed;
        //controller.uiPressAction.action.canceled += TriggerActionCanceled;
        leftControllerPos.performed += LeftPositionPerformed;

        joystick.performed += TranslateActionPerformed;
        joystick.canceled += TranslateActionCanceled;

        mainButton.performed += MainButtonPerformed;
        mainButton.canceled += MainButtonCanceled;

        leftTriggerButton.performed += LeftTriggerActionPerformed;
        leftTriggerButton.canceled += LeftTriggerActionCanceled;

        rightTriggerButton.performed += RightTriggerActionPerformed;
        rightTriggerButton.canceled += RightTriggerActionCanceled;

        secondaryButton.performed += SecondaryButtonPerformed;
        secondaryButton.canceled += SecondaryButtonCanceled;

        rightControllerPos.performed += RightPositionPerformed;

        leftGripButton.performed += LeftGripButtonPerformed;
        leftGripButton.canceled += LeftGripButtonCancel;

        rightGripButton.performed += RightGripButtonPerformed;
        rightGripButton.canceled += RightGripButtonCancel;
    }

    private void LeftGripButtonCancel(InputAction.CallbackContext obj)
    {
        gripPressedValue = 0f;
    }

    private void RightGripButtonPerformed(InputAction.CallbackContext obj)
    {
        rightGripValue = rightGripButton.ReadValue<float>();
    }

    private void RightGripButtonCancel(InputAction.CallbackContext obj)
    {
        rightGripValue = 0f;
    }

    private void LeftGripButtonPerformed(InputAction.CallbackContext obj)
    {
        gripPressedValue = leftGripButton.ReadValue<float>();
        
    }

    private void LeftPositionPerformed(InputAction.CallbackContext obj)
    {
        leftHandPos = leftControllerPos.ReadValue<Vector3>();
    }

    private void RightPositionPerformed(InputAction.CallbackContext obj)
    {
        rightHandPos = rightControllerPos.ReadValue<Vector3>();
    }

    private void LeftTriggerActionCanceled(InputAction.CallbackContext obj)
    {
        leftTriggerPressed = false;
    }

    private void LeftTriggerActionPerformed(InputAction.CallbackContext obj)
    {
        leftTriggerPressed = true;
    }


    private void RightTriggerActionCanceled(InputAction.CallbackContext obj)
    {
        rightTriggerPressed = false;
    }

    private void RightTriggerActionPerformed(InputAction.CallbackContext obj)
    {
        rightTriggerPressed = true;
    }

    public void SetMainButtonDown(bool mainButtonDown)
    {
        this.mainButtonDown = mainButtonDown;
    }

    private void Update()
    {
       // mainButtonDown = false;
       // mainButtonReleased = false;
    }

    //private void LateUpdate()
    //{
    //    if (mainButtonReleased)
    //    {
    //        //Debug.Log("True");
    //    }


    //    mainButtonReleased = false;
    //    secondaryButtonReleased = false;
    //}

    // private void Update()
    // {
    //     triggerButtonReleased = false;
    //}

    private void TriggerActionPerformed(InputAction.CallbackContext obj)
    {
        triggerButtonPressed = true;

    }

    private void TriggerActionCanceled(InputAction.CallbackContext obj)
    {
        triggerOccupied = false;
        triggerButtonPressed = false;
        triggerButtonReleased = true;
        StartCoroutine(TurnOffTriggerButtonRelease(0.1f));
    }

    private void ActivateActionPerformed(InputAction.CallbackContext obj)
    {


    }

    private void ActivateActionCanceled(InputAction.CallbackContext obj)
    {
        activationPressed = false;
    }

    private void SecondaryButtonCanceled(InputAction.CallbackContext obj)
    {
        secondaryButtonPressed = false;
        secondaryButtonReleased = true;
        StartCoroutine(TurnOffSecondaryButtonRelease(0.00001f));
    }

    private void SecondaryButtonPerformed(InputAction.CallbackContext obj)
    {
        secondaryButtonPressed = true;
        //character.GetComponent<CharacterMovementWithAnimations>().Attack();
    }

    private void MainButtonCanceled(InputAction.CallbackContext obj)
    {
        mainButtonPressed = false;

        if (!ignoreMainButtonRelease)
            mainButtonReleased = true;
        else
            ignoreMainButtonRelease = false;
    }

    private void MainButtonPerformed(InputAction.CallbackContext obj)
    {
        if (!mainButtonPressed)
            mainButtonDown = true;
        mainButtonPressed = true;
    }

    

    private void SelectionActionCanceled(InputAction.CallbackContext obj)
    {
        gripPressedValue = 0f;
        selectPressed = false;
    }

    private void SelectionActionPerformed(InputAction.CallbackContext obj)
    {
        selectPressed = true;
        //  gripPressedValue = controller.selectAction.action.ReadValue<float>();

    }

    /*
    * All controller actions placed here
    */


    private void TranslateActionPerformed(InputAction.CallbackContext obj)
    {
        // handObject.transform.GetChild(1).gameObject.SetActive(false);
        Vector2 vec2 = joystick.ReadValue<Vector2>();
        joystickDirection = transform.TransformDirection(new Vector3(vec2.x, 0.0f, vec2.y));
        joystickDirection.y = 0f;
    }

    private void TranslateActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        joystickDirection = Vector3.zero;
        //  lineRenderer.enabled = true;
        //  handObject.GetComponent<ArrowMovement>().SetEnabled(false);
        //  handObject.transform.GetChild(1).gameObject.SetActive(true);
        // character.GetComponent<CharacterMovementWithAnimations>().EnterIdle();
    }

    private void UIActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ResetAttributes();
        // SwitchStates();
    }

    /*
    * All external interactions placed here, might need to be modified later however.
    */

    private void ResetAttributes()
    {
    }

    /**
    private void SwitchStates()
    {
        if (handState == HandState.INTERACT)
        {
            handObject.transform.GetChild(1).gameObject.SetActive(false);
            handState = HandState.PLAYER;
        }
        else if (handState == HandState.PLAYER)
        {
            handObject.transform.GetChild(1).gameObject.SetActive(true);
            handState = HandState.INTERACT;
        }
    }
    **/

    //private void MoveCharacter()
    //{
    //    Vector2 vec2 = controller.translateAnchorAction.action.ReadValue<Vector2>();
    //    Vector3 vec3 = transform.TransformDirection(new Vector3(vec2.x, 0.0f, vec2.y));
    //    vec2.x = vec3.x;
    //    vec2.y = vec3.z;
    //    character.GetComponent<CharacterMovementWithAnimations>().MoveCharacter(vec2);
    //}

    public void IgnoreMainButtonNextRelease()
    {
        ignoreMainButtonRelease = true;
    }

    IEnumerator TurnOffMainButtonRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mainButtonReleased = false;
    }

    IEnumerator TurnOffTriggerButtonRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        triggerButtonReleased = false;
    }

    IEnumerator TurnOffSecondaryButtonRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        secondaryButtonReleased = false;
    }

    void OnEnable()
    {
        mainButton.Enable();
        secondaryButton.Enable();
        leftTriggerButton.Enable();
        joystick.Enable();
        leftControllerPos.Enable();

        rightTriggerButton.Enable();
        rightControllerPos.Enable();

        leftGripButton.Enable();
        rightGripButton.Enable();
    }

    void OnDisable()
    {
        mainButton.Disable();
        secondaryButton.Disable();
        leftTriggerButton.Disable();
        joystick.Disable();
        leftControllerPos.Disable();

        rightTriggerButton.Disable();
        rightControllerPos.Disable();

        leftGripButton.Disable();
        rightGripButton.Disable();
    }
}
