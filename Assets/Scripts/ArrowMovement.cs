using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        SetEnabled(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FacePosition(Vector2 toFace)
    {
        Vector3 newPos = new Vector3(toFace.x, transform.position.y, toFace.y);
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 facePos = currentPos + newPos;
        facePos.y = transform.position.y;
        transform.LookAt(facePos);
    }

    public void SetEnabled(bool enabled)
    {
        transform.GetChild(0).gameObject.SetActive(enabled);
    }
}
