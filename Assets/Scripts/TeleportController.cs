using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private XRInteractorLineVisual lineVisual;
    private XRRayInteractor xRRayInteractor;
    public GameObject teleportReticle;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineVisual = GetComponent<XRInteractorLineVisual>();
        xRRayInteractor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionController.Instance.gripPressedValue > 0.2f)
        {
            //Debug.Log(ActionController.Instance.gripPressedValue);
            lineRenderer.enabled = true;
            lineVisual.enabled = true;
            xRRayInteractor.enabled = true;
            teleportReticle.SetActive(true);
        } 
        else
        {
            lineRenderer.enabled = false;
            lineVisual.enabled = false;
            xRRayInteractor.enabled = false;
            teleportReticle.SetActive(false);
        }
            
    }
}
