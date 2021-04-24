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
    private float verticalVel;
    private float gravity = 4f;
    private bool grounded;
    private float glideFactor;
    private bool falling;
    public float glideForceFactor;
   


    // Start is called before the first frame update
    void Start()
    {
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
        if (grounded)
        {
            verticalVel = -gravity * Time.fixedDeltaTime;
            if (ActionController.Instance.MainButtonReleased)
                verticalVel = jumpForce * 0.5f;
            
            moveDir = new Vector3( ActionController.Instance.JoystickDirection.x, verticalVel, ActionController.Instance.JoystickDirection.z);
            Move(moveDir);
        } else
        {
            //slowly increasing jump velocity unitil reach max jump force
            if (verticalVel < jumpForce && !falling)
                verticalVel += 0.5f;
            else
            {
                falling = true;

                //holding jump button changes verticalVel based on glideFactor
                glideFactor = ActionController.Instance.MainButtonPressed ? glideForceFactor : 1f;
                    
                verticalVel -= gravity * Time.fixedDeltaTime;
                verticalVel *= glideFactor;
                
            }
        }
        Vector3 jumpDir = new Vector3(ActionController.Instance.JoystickDirection.x * 0.5f, verticalVel, ActionController.Instance.JoystickDirection.z * 0.5f);
        FacePosition(ActionController.Instance.JoystickDirection);
        rigidBody.velocity = jumpDir;



        if (GroundCheck() && ActionController.Instance.SecondaryButtonPressed)
        {
            anim.SetTrigger("attack");
        }

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
            
            rigidBody.MovePosition(transform.position + moveDir * speed * Time.fixedDeltaTime);
            FacePosition(ActionController.Instance.JoystickDirection);
        }
        else
        {
            anim.SetTrigger("idle");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
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

    private bool GroundCheck()
    {
        return Physics.Raycast(groundCheckTransform.position, -transform.up, groundCheckDistance, LayerMask.GetMask("Default"));
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
        Vector3 antiGravity = Physics.gravity * -jumpForce;
        rigidBody.AddForce(antiGravity, ForceMode.Impulse);
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