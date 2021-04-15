using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Rigidbody rigidBody;
    private Vector3 dir;
    private bool jumped;
    private float glideForce;
    public float glideDistanceFactor;
    private float glideSpeed;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(Physics.gravity);
        if (ActionController.Instance.ActivationPressed && !jumped)
        {
           
            Jump();
            glideForce = jumpForce;
        }

        if (jumped && ActionController.Instance.ActivationPressed)
        {
            if (glideForce > 0f)
                glideForce -= 0.6f;
            else
                glideForce = -0.5f;

            //  if (limit > 0f)
            //     limit = -0.0001f;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, glideForce, rigidBody.velocity.z);

            //   Physics.gravity = new Vector3(0f, limit, 0f);
            rigidBody.MovePosition(transform.position + dir * speed * glideSpeed * Time.fixedDeltaTime);
        }
        else
            rigidBody.MovePosition(transform.position + dir * speed * glideSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        glideSpeed = 1f;
        if (!ActionController.Instance.ActivationPressed)
            jumped = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        glideSpeed = glideDistanceFactor;
        jumped = true;
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