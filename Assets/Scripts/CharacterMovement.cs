using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //public Vector3 dir;
    public float speed;
    public float jumpForce;
    public Rigidbody rigidBody;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        rigidBody.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
    }

    public void MoveCharacter(Vector2 dir)
    {
        this.dir = new Vector3(dir.x, 0, dir.y);
        //Debug.Log(dir);
        FacePosition(dir);

        //transform.Translate(actualDir * speed * Time.fixedDeltaTime, Space.World);
        //  rigidBody.velocity = actualDir * speed * Time.fixedDeltaTime;
      //  rigidBody.MovePosition(transform.position + actualDir * speed * Time.fixedDeltaTime);
    }

    public void FacePosition(Vector2 toFace)
    {
        Vector3 newPos = new Vector3(toFace.x, transform.position.y, toFace.y);
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 facePos = currentPos + newPos;
        facePos.y = transform.position.y;
        transform.LookAt(facePos);
    }

    //Jump is called by action controller script
    public void Jump()
    {
        rigidBody.AddForce(0, jumpForce, 0f, ForceMode.Impulse);
    }
}