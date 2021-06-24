using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDetector : MonoBehaviour
{
    public float faceMoveSpeed;
    public float rotSpeed;
    public GameObject indicatorSphere;
    ActionController actionController;
    private Vector3 lastHandPos;
    private Quaternion lastHandRotation;
    private GameObject detectedObject;
    private Vector3 hitNormal;
    Vector3 cameraPoint = Vector3.zero;
    public LineRenderer lineRenderer;
    private LayerMask ignoreLayer;
    private InteractiveAimReticle interactiveAimReticle;

    public enum Hand { LEFT, RIGHT};
    public Hand hand;
    private bool triggerPressed;
    public float length;
    private float offsetAmount = 0.0015f;

    // Start is called before the first frame update
    void Start()
    {
        interactiveAimReticle = indicatorSphere.GetComponent<InteractiveAimReticle>();
        ignoreLayer = ~LayerMask.GetMask("Ignore");
        actionController = GetComponent<ActionController>();
      //  lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        int layer =  LayerMask.GetMask("GrabThrow") | LayerMask.GetMask("Interactable") | LayerMask.GetMask("GravityInteractable") | LayerMask.GetMask("IgnoreInteractable") | LayerMask.GetMask("UI");


        if (hand == Hand.LEFT)
        {
            triggerPressed = ActionController.Instance.LeftTriggerPressed;
        }
        if (hand == Hand.RIGHT)
            triggerPressed = ActionController.Instance.RightTriggerPressed;

        if (Physics.Raycast(transform.position, transform.forward, out hit, length, layer))
       // if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            //ProBuilder API wants Screen Pos
            cameraPoint = Camera.main.WorldToScreenPoint(hit.point);
            hitNormal = hit.normal;
            //show where the ray hit on object's surface
            SetIndicatorPos(hit.point);
            DrawIndicatorRay(transform.position, hit.point, 0.005f);

            if (!interactiveAimReticle.hand)
            {
                GrabThrow grabThrow = hit.transform.gameObject.GetComponent<GrabThrow>();

                if (grabThrow == null || grabThrow.canBeGrabThrown)
                {
                   interactiveAimReticle.TurnOnHand();
                }
            }

            if (detectedObject == null && triggerPressed )
            {
              //  ActionController.Instance.TriggerOccupied = true;
                detectedObject = hit.transform.gameObject;
            }
        } else {

            if (!interactiveAimReticle.turnedOff)
            {
                interactiveAimReticle.TurnOff();
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, length, ignoreLayer))
            {
                hitNormal = hit.normal;
                //show where the ray hit on object's surface
                SetIndicatorPos(hit.point);
                DrawIndicatorRay(transform.position, hit.point, 0.005f);
            }
            else
            {
                DrawIndicatorRay(transform.position, transform.position + transform.forward * length, 0.005f);
                SetIndicatorPos(transform.position + transform.forward * length);
            }
        }

        if (triggerPressed && detectedObject != null)
        {
            ObjectDetection();
        }
        else if (!triggerPressed && detectedObject != null)
        {
            RaycastHit shootHit;
            if ((detectedObject.CompareTag("Throwable") || detectedObject.CompareTag("Crate") || detectedObject.CompareTag("Shuriken")) && detectedObject.GetComponent<GrabThrow>().canBeGrabThrown)
            {
                if (Physics.Raycast(transform.position, transform.forward, out shootHit, 10))
                {
                    ReleaseObject(detectedObject, shootHit.point);
                }
                else
                {
                    ReleaseObject(detectedObject, transform.position + transform.forward * 10);
                }
                    
            }
            detectedObject = null;
        }

        if (hand == Hand.LEFT)
            lastHandPos = ActionController.Instance.LeftHandPos;

        if (hand == Hand.RIGHT)
            lastHandPos = ActionController.Instance.RightHandPos;
      //  lastHandRotation = transform.rotation;
    }

    void ObjectDetection()
    {
        if (detectedObject.CompareTag("Grabable") || detectedObject.CompareTag("GrabableNotWithPlayer"))
            MoveObj(detectedObject);
        if (detectedObject.CompareTag("Rotatable") || detectedObject.CompareTag("RotatableNotWithPlayer"))
            RotateObject(detectedObject);

        if (detectedObject.CompareTag("Reset"))
            detectedObject.GetComponent<RestPlayer>().Reset();

        if (detectedObject.CompareTag("Tutorial"))
        {
            if (ActionController.Instance.LeftTriggerPressed || ActionController.Instance.RightTriggerPressed)
                SceneManager.LoadScene("Tutorial");
        }
        if (detectedObject.CompareTag("Campaign"))
        {
            if (ActionController.Instance.LeftTriggerPressed || ActionController.Instance.RightTriggerPressed)
                SceneManager.LoadScene("final_world");
        }

        if ((detectedObject.CompareTag("Throwable") || detectedObject.CompareTag("Crate") || detectedObject.CompareTag("Shuriken"))
            && detectedObject.GetComponent<GrabThrow>().canBeGrabThrown)
        {
            if (!interactiveAimReticle.reticle)
            {
                interactiveAimReticle.TurnOnReticle();
            }
            GrabObject(detectedObject);
        }
    }

    void RotateObject(GameObject obj)
    {
        //  Quaternion moveDir = new Quaternion(transform.rotation.x - lastHandRotation.x, transform.rotation.y - lastHandRotation.y,
        //      transform.rotation.z - lastHandRotation.z, 0f);
        Vector3 dir;

        if (hand == Hand.LEFT)
            dir = ActionController.Instance.LeftHandPos - lastHandPos;
        else
            dir = ActionController.Instance.RightHandPos - lastHandPos;

       // moveDir = transform.TransformDirection(moveDir);
        obj.GetComponent<InteractableCube>().Rotate(dir * 100f, rotSpeed);
    }

    void MoveObj(GameObject obj)
    {
        Vector3 dir;
        if (hand == Hand.LEFT)
            dir = ActionController.Instance.LeftHandPos - lastHandPos;
        else
            dir = ActionController.Instance.RightHandPos - lastHandPos;
        obj.GetComponent<InteractableCube>().SetAtNewPos(dir);
    }

    void DetectFace(GameObject obj)
    {
        if (!ActionController.Instance.MainButtonPressed)
        {
            //always updating lasthandPos, even select not pressed, or endup with huge diff
            lastHandPos = ActionController.Instance.HandPos;
            return;
        }
        //how much controller moved in one frame
        Vector3 diff = ActionController.Instance.HandPos - lastHandPos;
        Vector3 faceMoveDir = diff.normalized;
        float diffAmount = diff.magnitude;


        if (obj != null && detectedObject.CompareTag("Grabable"))
        {
            obj.GetComponent<InteractableCube>().Scale(Camera.main,
                cameraPoint, hitNormal, faceMoveDir, diffAmount * faceMoveSpeed, transform.position);
        }
        lastHandPos = ActionController.Instance.HandPos;
    }

    void SetIndicatorPos(Vector3 pos)
    {
        if (ActionController.Instance.gripPressedValue > 0)
            indicatorSphere.SetActive(false);
        else
            indicatorSphere.SetActive(true);

        Vector3 indicatorPosition = pos - (pos - transform.position).normalized * offsetAmount;

        indicatorSphere.transform.position = indicatorPosition;
        indicatorSphere.transform.rotation = transform.rotation;
        //indicatorSphere.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
    }

    void DrawIndicatorRay(Vector3 startPos, Vector3 endPos, float width)
    {
        if (ActionController.Instance.gripPressedValue > 0)
            lineRenderer.enabled = false;
        else
            lineRenderer.enabled = true;

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos - (endPos - transform.position).normalized * offsetAmount);
    }

    void GrabObject(GameObject obj)
    {
        obj.GetComponent<GrabThrow>().CallMoveToHandInit();
        obj.GetComponent<GrabThrow>().MoveToHand(transform);
    }

    void ReleaseObject(GameObject obj, Vector3 releasePosition)
    {
        obj.GetComponent<GrabThrow>().CallMoveToReleasePositionInit();
        obj.GetComponent<GrabThrow>().MoveToReleasePosition(releasePosition);
    }
}