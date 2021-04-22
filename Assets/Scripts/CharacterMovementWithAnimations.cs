using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementWithAnimations : MonoBehaviour
{
    //public Vector3 dir;
    public float speed;
    public float jumpForce;
    public Rigidbody rigidBody;
    public Animator anim;
    private Vector3 dir;
    public ActionController ac;

    public GameObject leftAxe;
    public GameObject rightAxe;

    Rigidbody rb;
    Vector3 force;
    Vector3 alignWithNormal;
    private bool isGrounded;
    private bool jumped;
    private float yVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                force = -hit.normal;
                alignWithNormal = hit.normal;
            }

        }
       
        Quaternion q = Quaternion.FromToRotation(Vector3.up, alignWithNormal);
        Quaternion rotation = q; 
        Vector3 rotateVector = rotation * dir;


        transform.rotation = Quaternion.LookRotation(transform.up, alignWithNormal);


        //if (isGrounded && ActionController.Instance.MainButtonPressed)
        //{
        //    isGrounded = false;
        //    Debug.Log("once only");
        //    rb.AddForce(-force, ForceMode.Impulse);
        //}   else 
        //    rb.AddForce(force);
       

       
        rb.AddForce(force);

          
        
        transform.rotation = Quaternion.LookRotation(rotateVector, alignWithNormal);
        rigidBody.MovePosition(transform.position + rotateVector * speed * Time.fixedDeltaTime);






        if (dir == Vector3.zero && !anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("idle");
        }

        
        //else if (Input.GetKeyDown(KeyCode.B))
        //{
        //    anim.SetTrigger("idle");
        //}

    }

    public void Attack()
    {
        anim.SetTrigger("attack");
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        jumped = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }


    public void MoveCharacter(Vector2 dir)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            anim.SetTrigger("run");
        }
            

        

        this.dir = new Vector3(dir.x, 0, dir.y);
        
      //  FacePosition(dir);

        //transform.Translate(actualDir * speed * Time.fixedDeltaTime, Space.World);
        //  rigidBody.velocity = actualDir * speed * Time.fixedDeltaTime;
      //  rigidBody.MovePosition(transform.position + actualDir * speed * Time.fixedDeltaTime);
    }

    public void EnterIdle()
    {
        anim.SetTrigger("idle");
    }

    public void FacePosition(Vector2 toFace)
    {
        //Vector3 newPos = new Vector3(toFace.x, transform.position.y, toFace.y);
        //Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //Vector3 facePos = currentPos + newPos;
        //facePos.y = transform.position.y;
        //   Vector3 face = new Vector3(toFace.x, 0f, toFace.y);
        //  transform.LookAt(new Vector3(toFace.x, 0f, toFace.y));
        //  transform.rotation = Quaternion.LookRotation(face, alignWithNormal);
        // 
        transform.rotation = Quaternion.LookRotation(new Vector3(toFace.x, 0f, toFace.y), alignWithNormal);
    }

    //Jump is called by action controller script
    public void Jump()
    {
       // rigidBody.AddForce(0, jumpForce, 0f, ForceMode.Impulse);
    }

    public void TurnOnAxeCollsions(string hand, bool turnOn)
    {
        if (hand.Equals("left"))
        {
            if (turnOn)
                leftAxe.GetComponent<AxeCollision>().TurnOnCollider();
            else
                leftAxe.GetComponent<AxeCollision>().TurnOffCollider();
        }
        else if (hand.Equals("right"))
        {
            if (turnOn)
                rightAxe.GetComponent<AxeCollision>().TurnOnCollider();
            else
                rightAxe.GetComponent<AxeCollision>().TurnOffCollider();
        }
        
    }

    public void TurnOnLeftAxe()
    {
        leftAxe.GetComponent<AxeCollision>().TurnOnCollider();
    }

    public void TurnOffLeftAxe()
    {
        leftAxe.GetComponent<AxeCollision>().TurnOffCollider();
    }

    public void TurnOnRightAxe()
    {
        rightAxe.GetComponent<AxeCollision>().TurnOnCollider();
    }

    public void TurnOffRightAxe()
    {
        rightAxe.GetComponent<AxeCollision>().TurnOffCollider();
    }
}