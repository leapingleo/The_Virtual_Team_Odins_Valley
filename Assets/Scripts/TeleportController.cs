using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private XRInteractorLineVisual lineVisual;
    private XRRayInteractor xRRayInteractor;
    public GameObject teleportReticle;
    public GameObject exitGameButton;
   // public GameObject teleportReticle;
    // Start is called before the first frame update
    void Start()
    {
       // lineRenderer = GetComponent<LineRenderer>();
        lineVisual = GetComponent<XRInteractorLineVisual>();
        xRRayInteractor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionController.Instance.gripPressedValue > 0f)
        {
            //Debug.Log(ActionController.Instance.gripPressedValue);
            lineRenderer.enabled = false;
            lineVisual.enabled = true;
            xRRayInteractor.enabled = true;
            teleportReticle.SetActive(true);
            //teleportReticle.SetActive(true);
        } 
        else
        {
            lineRenderer.enabled = true;
            lineVisual.enabled = false;
            xRRayInteractor.enabled = false;
            teleportReticle.SetActive(false);
            //  teleportReticle.SetActive(false);
        }

        if (ActionController.Instance.rightGripValue > 0f)
            exitGameButton.SetActive(true);
        else
            exitGameButton.SetActive(false);
    }
}
