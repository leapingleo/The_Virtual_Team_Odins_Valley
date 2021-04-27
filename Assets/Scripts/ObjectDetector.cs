using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public float faceMoveSpeed;
    public float rotSpeed = 100f;
    public GameObject indicatorSphere;
    ActionController actionController;
    private Vector3 lastHandPos;
    private Quaternion lastHandRotation;
    private GameObject detectedObject;
    private Vector3 hitNormal;
    Vector3 cameraPoint = Vector3.zero;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        actionController = GetComponent<ActionController>();
      //  lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        int layer = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(transform.position, transform.forward, out hit, 3, layer))
        {
            //ProBuilder API wants Screen Pos
            cameraPoint = Camera.main.WorldToScreenPoint(hit.point);
            hitNormal = hit.normal;
            //show where the ray hit on object's surface
            SetIndicatorPos(hit.point);
            DrawIndicatorRay(transform.position, hit.point, 0.01f);


            if (detectedObject == null && ActionController.Instance.TriggerButtonPressed && !ActionController.Instance.TriggerOccupied)
            {
                ActionController.Instance.TriggerOccupied = true;
                detectedObject = hit.transform.gameObject;
            }
        } else {
            lineRenderer.enabled = false;
            SetIndicatorPos(new Vector3(999, 999, 999));
        }

        if (ActionController.Instance.TriggerButtonPressed && detectedObject != null)
        {
            if (detectedObject.CompareTag("Grabable") || detectedObject.CompareTag("GrabableNotWithPlayer"))
                MoveObj(detectedObject);
            if (detectedObject.CompareTag("Rotatable") || detectedObject.CompareTag("RotatableNotWithPlayer"))
                RotateObject(detectedObject);

            if (detectedObject.CompareTag("Reset"))
                detectedObject.GetComponent<RestPlayer>().Reset();
        } 
        else {
            detectedObject = null;
        //    ActionController.Instance.TriggerOccupied = false;
        }
        lastHandPos = ActionController.Instance.HandPos;
        lastHandRotation = transform.rotation;
    }

    void RotateObject(GameObject obj)
    {
        Quaternion moveDir = new Quaternion(transform.rotation.x - lastHandRotation.x, transform.rotation.y - lastHandRotation.y,
            transform.rotation.z - lastHandRotation.z, 0f);
        
        Vector3 diff = ActionController.Instance.HandPos - lastHandPos;
        //Vector3 moveDir = diff * 100f;
        
        obj.GetComponent<InteractableCube>().Rotate(moveDir, rotSpeed);
    }

    void MoveObj(GameObject obj)
    {
        Vector3 dir = ActionController.Instance.HandPos - lastHandPos;
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
        indicatorSphere.transform.position = pos;
        
        indicatorSphere.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
    }

    void DrawIndicatorRay(Vector3 startPos, Vector3 endPos, float width)
    {
        lineRenderer.enabled = true;

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}