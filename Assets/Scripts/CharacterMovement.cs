using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovement : MonoBehaviour
{
   // private ActionBasedController leftController;
   // private ActionBasedController rightController;

    private float speed;
    public float jumpForce;
    public Rigidbody rigidBody;
    private bool jumping;
    private float glideForce;
    public float glideDistanceFactor;
    public float runTrigger;
    public float glideSpeed;
    public float groundCheckDistance;
    private bool gliding;
    private bool canCheckGround;
    public float moveSpeed;
    public float attackSpeed;

   // public bool yiyiHanded;
   // public bool tristanHanded;
    public Animator anim;
    public Transform groundCheckTransform;
   // public ActionController leftActionController;
   // public ActionController rightActionController;
   // private ActionController acMovement;
   // private ActionController acButtons;

    public GameObject leftAxe;
    public GameObject rightAxe;
    private Vector3 moveDir;
    private Vector3 planeVerticalVel;
    private Vector3 gravity;
    private bool grounded;
    private float glideFactor;
    private bool falling;
    public float glideForceFactor;
    private Vector3 groundNormal;
    Quaternion alignWithSurfaceRot;

    private bool usingCustomGravity = false;


    // Start is called before the first frame update
    void Start()
    {
       // verticalVel = new Vector3(-1f, 1f, 0f);
        gliding = false;
        jumping = false;
        canCheckGround = false;

        /*
        if (tristanHanded)
        {
            acMovement = leftActionController;
            acButtons = rightActionController;
        }
        
        if (yiyiHanded)
        {
            acMovement = rightActionController;
            acButtons = leftActionController;
        }
        **/


    }

    //void FixedUpdate()
    //{
    //    if (!jumping)
    //    {
    //        Move();
    //    }


    //    if (!jumping && ActionController.Instance.MainButtonReleased)
    //    {
    //        //Debug.Log("True");
    //        Jump();
    //    }

    //    if (jumping)
    //    {
    //        /*
    //         * HAVENT USED IT BECUASE GLIDE DIDNT WORK FOR ME, I THINK MESSED IT UP, SORRY YIYI ):
    //         */
    //        //if (acButtons.MainButtonPressed)
    //        //{
    //        //    Glide();
    //        //}
    //        //else
    //        //{
    //        //    Fall();
    //        //}
    //        Fall();

    //        if (canCheckGround)
    //        {
    //            if (GroundCheck())
    //            {
    //                if (gliding)
    //                {
    //                    SetGliding(false);
    //                }

    //                jumping = false;
    //                anim.SetTrigger("jump_end");
    //                canCheckGround = false;
    //            }
    //        }
    //    }

    //    if (!jumping && ActionController.Instance.SecondaryButtonReleased)
    //    {
    //        anim.SetTrigger("attack");
    //    }
    //}  

    void FixedUpdate()
    {
        //  GroundCheck();
         JumpOnDifferentGravity();

        /*
        if (grounded && ActionController.Instance.MainButtonReleased)
        {
            rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (ActionController.Instance.MainButtonPressed)
        {
            float glidingY = rigidBody.velocity.y * 0.2f;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, glidingY, rigidBody.velocity.z);
        }

        Move(ActionController.Instance.JoystickDirection);
        */

        if (grounded && ActionController.Instance.SecondaryButtonPressed)
        {
            anim.SetTrigger("attack");
        }
    }

    void JumpOnDifferentGravity()
    {
        rigidBody.useGravity = false;
       
        alignWithSurfaceRot = Quaternion.FromToRotation(Vector3.up, groundNormal);
        moveDir = new Vector3(ActionController.Instance.JoystickDirection.x, 0, ActionController.Instance.JoystickDirection.z);

        if (grounded)
        {
            planeVerticalVel = -gravity * Time.fixedDeltaTime;

            if (ActionController.Instance.MainButtonReleased)
                planeVerticalVel = groundNormal * jumpForce * 0.5f;

            moveDir = moveDir.normalized + planeVerticalVel;
            //rotate the move direction
            moveDir = alignWithSurfaceRot * moveDir;

            Move(moveDir);
        }
        else
        {
            falling = true;
            //holding jump button changes verticalVel based on glideFactor
            glideFactor = ActionController.Instance.MainButtonPressed ? glideForceFactor : 1f;
            planeVerticalVel -= gravity * Time.fixedDeltaTime;
            planeVerticalVel *= glideFactor;
        }
        Vector3 jumpDir = Quaternion.FromToRotation(Vector3.up, groundNormal) * ActionController.Instance.JoystickDirection * 0.5f +
            planeVerticalVel;

        if (falling)
        {
            if (ActionController.Instance.JoystickDirection.magnitude <= 0f)
                transform.rotation = alignWithSurfaceRot;
            else //moving while gliding, transform joystick direction so it aligns with the plane
                transform.rotation = Quaternion.LookRotation(alignWithSurfaceRot * ActionController.Instance.JoystickDirection, groundNormal);
        }
        rigidBody.velocity = jumpDir;
    }

    public void Move(Vector3 dir)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack01") || anim.GetCurrentAnimatorStateInfo(0).IsName("attack02"))
        {
            speed = attackSpeed;
        }
        else
        {
            speed = moveSpeed;
        }

        if (ActionController.Instance.JoystickDirection != Vector3.zero)
        {
            anim.SetTrigger("walk");


            transform.rotation = Quaternion.LookRotation(dir, groundNormal);
            rigidBody.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
           
           // FacePosition(ActionController.Instance.JoystickDirection);
        }
        else
        {
            transform.rotation = alignWithSurfaceRot;
         //   rigidBody.angularVelocity = Vector3.zero;
            anim.SetTrigger("idle");
        }
    }

    
    private void OnCollisionStay(Collision collision)
    {
        groundNormal = collision.contacts[0].normal;
        gravity = groundNormal * 4f;
        planeVerticalVel = groundNormal;
        falling = false;
        glideFactor = 0.1f;
        grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
    

    public void SetGliding(bool gliding)
    {
        if (gliding)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            this.gliding = gliding;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            this.gliding = false;
        }
    }

    public void Glide()
    {
        if (!gliding)
        {
            SetGliding(true);
        }

        if (glideForce > 0f)
            glideForce -= 0.6f;
        else
            glideForce = -0.5f;

        if (ActionController.Instance.JoystickDirection != Vector3.zero)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, glideForce, rigidBody.velocity.z);
            rigidBody.MovePosition(transform.position + ActionController.Instance.JoystickDirection * speed * glideSpeed * Time.fixedDeltaTime);
            FacePosition(ActionController.Instance.JoystickDirection);
        }
    }

    public void Fall()
    {
        if (gliding)
        {
            SetGliding(false);
        }

        if (ActionController.Instance.JoystickDirection != Vector3.zero)
        {
            
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z);
            rigidBody.velocity += ActionController.Instance.JoystickDirection * 0.001f;
            //rigidBody.velocity = 
            //rigidBody.MovePosition(transform.position + acMovement.JoystickDirection * speed * glideSpeed * Time.fixedDeltaTime);
            FacePosition(ActionController.Instance.JoystickDirection);
        }
        
    }

    /*
    private bool GroundCheck()
    {
        return Physics.Raycast(groundCheckTransform.position, -transform.up, groundCheckDistance, LayerMask.GetMask("Default"));
    }
    */
    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheckTransform.position, -transform.up, out hit, groundCheckDistance, LayerMask.GetMask("Default")))
        {
            grounded = true;

            if (usingCustomGravity)
            {
                groundNormal = hit.normal;
                gravity = groundNormal * 4f;
                planeVerticalVel = groundNormal;
                falling = false;
                glideFactor = 0.1f;
            } else
            {
                groundNormal = Vector3.up;
                planeVerticalVel = Vector3.up;
            }
        }
        else
        {
            grounded = false;
        }
    }

private void EnterGround()
    {
        SetGliding(false);
        jumping = false;
        anim.SetTrigger("jump_end");
    }

    public void FacePosition(Vector3 toFace)
    {
        transform.rotation = Quaternion.LookRotation(toFace);
    }

    //Jump is called by action controller script
    public void Jump()
    {
      //  Vector3 antiGravity = Physics.gravity * -jumpForce;
       // rigidBody.AddForce(antiGravity, ForceMode.Impulse);
        jumping = true;
        anim.SetTrigger("jump_start");
        StartCoroutine(StartToCheckGround(0.001f));
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

    /*
     * THIS NEEDS TO BE REMOVED TOO LAZY SORRY
     */
    IEnumerator StartToCheckGround(float timeTillCheckGround)
    {
        yield return new WaitForSeconds(timeTillCheckGround);
        canCheckGround = true;
    }

}