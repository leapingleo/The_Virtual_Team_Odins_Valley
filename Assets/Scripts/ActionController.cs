﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionController : MonoBehaviour
{
    public InputAction mainButton;
    public InputAction secondaryButton;
    enum HandState { PLAYER, INTERACT };
    private HandState handState;
    public GameObject handObject;
    public GameObject character;
    private ActionBasedController controller;
    private LineRenderer lineRenderer;
    private ObjectDetector objectDetector;
    private bool mainButtonPressed;
    public bool MainButtonPressed { get { return mainButtonPressed; } }
    private bool secondaryButtonPressed;
    public bool SecondaryButtonPressed { get { return secondaryButtonPressed; } }
    private bool selectPressed;
    public bool SelectPressed { get { return selectPressed; } }
    private Vector3 handPos;
    public Vector3 HandPos { get { return handPos; } }

    void Awake()
    {
        handState = HandState.PLAYER;

        handObject.transform.GetChild(1).gameObject.SetActive(true);
        lineRenderer = GetComponent<LineRenderer>();
        objectDetector = GetComponent<ObjectDetector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        controller = GetComponent<ActionBasedController>();

        controller.translateAnchorAction.action.performed += TranslateActionPerformed;
        controller.translateAnchorAction.action.canceled += TranslateActionCanceled;

        controller.activateAction.action.performed += ActivateActionPerformed;

        // pushing down the joystick will activate this.
        controller.uiPressAction.action.performed += UIActionPerformed;

        controller.selectAction.action.performed += SelectionActionPerformed;
        controller.selectAction.action.canceled += SelectionActionCanceled;
        controller.positionAction.action.performed += PositionPerformed;
        mainButton.performed += MainButtonPerformed;
        mainButton.canceled += MainButtonCanceled;

        secondaryButton.performed += SecondaryButtonPerformed;
        secondaryButton.canceled += SecondaryButtonCanceled;
    }

    private void SecondaryButtonCanceled(InputAction.CallbackContext obj)
    {
        secondaryButtonPressed = false;
    }

    private void SecondaryButtonPerformed(InputAction.CallbackContext obj)
    {
        secondaryButtonPressed = true;
    }

    private void MainButtonCanceled(InputAction.CallbackContext obj)
    {
        mainButtonPressed = false;
    }

    private void MainButtonPerformed(InputAction.CallbackContext obj)
    {
        mainButtonPressed = true;
    }

    private void PositionPerformed(InputAction.CallbackContext obj)
    {
        handPos = controller.positionAction.action.ReadValue<Vector3>();
    }

    private void SelectionActionCanceled(InputAction.CallbackContext obj)
    {
        selectPressed = false;
    }

    private void SelectionActionPerformed(InputAction.CallbackContext obj)
    {
        selectPressed = true;

    }

    /*
    * All controller actions placed here
    */
    private void ActivateActionPerformed(InputAction.CallbackContext obj)
    {
        if (handState == HandState.PLAYER)
        {
            character.GetComponent<CharacterMovement>().Jump();
        }
    }

    private void TranslateActionPerformed(InputAction.CallbackContext obj)
    {
        handObject.transform.GetChild(1).gameObject.SetActive(false);
       
        if (handState == HandState.PLAYER)
        {
            MoveCharacter();
        }
        else if (handState == HandState.INTERACT)
        {

        }
    }

    private void TranslateActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        lineRenderer.enabled = true;
        handObject.GetComponent<ArrowMovement>().SetEnabled(false);
        handObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void UIActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ResetAttributes();
        SwitchStates();
    }

    /*
    * All external interactions placed here, might need to be modified later however.
    */

    private void ResetAttributes()
    {
        handObject.GetComponent<HandObjectScript>().ResetRotation();
        handObject.GetComponent<ArrowMovement>().SetEnabled(false);
        character.GetComponent<CharacterMovement>().MoveCharacter(Vector2.zero);
    }

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

    private void MoveCharacter()
    {
        Vector2 vec2 = controller.translateAnchorAction.action.ReadValue<Vector2>();
        Vector3 vec3 = transform.TransformDirection(new Vector3(vec2.x, 0.0f, vec2.y));
        vec2.x = vec3.x;
        vec2.y = vec3.z;
        character.GetComponent<CharacterMovement>().MoveCharacter(vec2);
        MoveArrow(vec2);
    }

    private void MoveArrow(Vector2 dir)
    {
        handObject.GetComponent<ArrowMovement>().SetEnabled(true);
        handObject.GetComponent<ArrowMovement>().FacePosition(dir);
    }

    void OnEnable()
    {
        mainButton.Enable();
        secondaryButton.Enable();
    }

    void OnDisable()
    {
        mainButton.Disable();
        secondaryButton.Disable();
    }
}