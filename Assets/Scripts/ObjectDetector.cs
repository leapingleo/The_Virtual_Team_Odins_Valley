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
    private GameObject detectedObject;
    private Vector3 hitNormal;
    Vector3 cameraPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        actionController = GetComponent<ActionController>();
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

            if (detectedObject == null && actionController.MainButtonPressed)
            {
                detectedObject = hit.transform.gameObject;
            }
        } else {
            SetIndicatorPos(new Vector3(999, 999, 999));
        }

        if (actionController.MainButtonPressed && detectedObject != null)
        {
            if (detectedObject.CompareTag("Grabable"))
                MoveObj(detectedObject);
            if (detectedObject.CompareTag("Rotatable"))
                RotateObject(detectedObject);

            if (detectedObject.CompareTag("Reset"))
                detectedObject.GetComponent<RestPlayer>().Reset();
        } 
        else {
            detectedObject = null;
        }
        lastHandPos = actionController.HandPos;
    }

    void RotateObject(GameObject obj)
    {
        Vector3 diff = actionController.HandPos - lastHandPos;
        Vector3 moveDir = diff * 100f;
        
        obj.GetComponent<InteractableCube>().Rotate(moveDir, rotSpeed);
    }

    void MoveObj(GameObject obj)
    {
        Vector3 dir = actionController.HandPos - lastHandPos;
        obj.GetComponent<InteractableCube>().SetAtNewPos(dir);
    }

    void DetectFace(GameObject obj)
    {
        if (!actionController.MainButtonPressed)
        {
            //always updating lasthandPos, even select not pressed, or endup with huge diff
            lastHandPos = actionController.HandPos;
            return;
        }
        //how much controller moved in one frame
        Vector3 diff = actionController.HandPos - lastHandPos;
        Vector3 faceMoveDir = diff.normalized;
        float diffAmount = diff.magnitude;


        if (obj != null && detectedObject.CompareTag("Grabable"))
        {
            obj.GetComponent<InteractableCube>().Scale(Camera.main,
                cameraPoint, hitNormal, faceMoveDir, diffAmount * faceMoveSpeed, transform.position);
        }
        lastHandPos = actionController.HandPos;
    }

     void SetIndicatorPos(Vector3 pos)
    {
        indicatorSphere.transform.position = pos;
    }
}