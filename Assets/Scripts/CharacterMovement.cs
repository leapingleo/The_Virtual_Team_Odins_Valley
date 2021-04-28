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
    public float yiyiGravityModifier;
    public PlayerGrounded playerGrounded;
    private bool gliding;
    private bool canCheckGround;
    public float moveSpeed;
    public float attackSpeed;
    private bool buttonPressedSecondTime;

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
    private float gravity = 9.8f;
    private bool grounded;
    private float glideFactor;
    private bool falling;
    public float glideForceFactor;
    private float timeTillCheckGround = 0.25f;
    private float timer = 0.0f;
    private LayerMask ignorePlayer;

    public Transform crowGlideTransform;
    public Transform crowStandardTransform;
    public GameObject crow;

    private Vector3 prevPos;
    public float checkPointRate;
    private float checkPointTimer;

    private float prevYvel;
    private bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressedSecondTime = false;
        ignorePlayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore")); 
        timer = timeTillCheckGround;
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

    void FixedUpdate()
    {
        if (Time.time > checkPointTimer)
        {
            checkPointTimer += checkPointRate;

            if (grounded && transform.position.y > 0.6f)
                prevPos = transform.position;
        }

        if (transform.position.y < 0.3f)
            transform.position = prevPos;

        if (grounded)
        {
            verticalVel = -gravity * Time.fixedDeltaTime;
            if (ActionController.Instance.MainButtonPressed && canJump)
            {
                canJump = false;
                anim.SetTrigger("jump_start");
                //  rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                verticalVel = jumpForce ;
            }
          
        }else { 
          //  if (verticalVel < jumpForce && !falling)
           // {
           //     if (jumping)
           //     verticalVel += yiyiGravityModifier;
           // }
          ///  else
          //  {
            falling = true;
            //  if (ActionController.Instance.MainButtonPressed)
            // {
            if (glideFactor != 1f)
            {
                crow.transform.position = crowGlideTransform.position;
            }
            else
            {
                crow.transform.position = crowStandardTransform.position;

            }

            glideFactor = (ActionController.Instance.MainButtonPressed) ? glideForceFactor : 1f;
                  //  rigidBody.velocity = new Vector3(rigidBody.velocity.x, verticalVel * glideFactor, rigidBody.velocity.z);
                //  }
            verticalVel -= gravity * Time.fixedDeltaTime;

            if (prevYvel > verticalVel)
                verticalVel *= glideFactor;
                
           // }
        }
        prevYvel = verticalVel;
        rigidBody.velocity = new Vector3(0, verticalVel, 0);
        moveDir = new Vector3(ActionController.Instance.JoystickDirection.x, 0, ActionController.Instance.JoystickDirection.z);
        Move(moveDir.normalized);
        // Move(ActionController.Instance.JoystickDirection);

        /*
        if (playerGrounded.grounded)
        {
            verticalVel = -gravity * Time.fixedDeltaTime;
            if (ActionController.Instance.MainButtonReleased)
            {
                grounded = false;
                jumping = true;
                verticalVel = jumpForce * yiyiGravityModifier;
                anim.SetTrigger("jump_start");
                buttonPressedSecondTime = false;
            }
                
            
            moveDir = new Vector3( ActionController.Instance.JoystickDirection.x, verticalVel, ActionController.Instance.JoystickDirection.z);
            Move(moveDir);
        } 
        else
        {
            //slowly increasing jump velocity unitil reach max jump force
            if (verticalVel < jumpForce && !falling)
            {
                verticalVel += yiyiGravityModifier;
                jumping = true;
            }
            else
            {
                jumping = false;
                falling = true;

                //holding jump button changes verticalVel based on glideFactor
                glideFactor = (ActionController.Instance.MainButtonPressed) ? glideForceFactor : 1f; 

                //glideFactor = 1f;


                if (glideFactor != 1f)
                {
                    crow.transform.position = crowGlideTransform.position;
                }
                else
                {
                    crow.transform.position = crowStandardTransform.position;
                    
                }
                    
                verticalVel -= gravity * Time.fixedDeltaTime;
                verticalVel *= glideFactor;
                
            }
        }


        Vector3 jumpDir = new Vector3(ActionController.Instance.JoystickDirection.x * yiyiGravityModifier, verticalVel, ActionController.Instance.JoystickDirection.z * yiyiGravityModifier);
        rigidBody.velocity = jumpDir;
        */

        if (ActionController.Instance.JoystickDirection != Vector3.zero)
        {
            FacePosition(ActionController.Instance.JoystickDirection);
        }

        if (grounded && ActionController.Instance.SecondaryButtonPressed)
        {
            anim.SetTrigger("attack");
        }

        if ((jumping || falling) && grounded)
        {
            buttonPressedSecondTime = false;
            crow.transform.position = crowStandardTransform.position;
            Debug.Log("landed");
            anim.SetTrigger("jump_end");
            falling = false;
            
            glideFactor = 0.1f;
           // grounded = true;

            //if character gliding then reaches the ground, as soon as he realeases he will jump again which feels weird.
           // if (ActionController.Instance.MainButtonPressed)
           // {
           //     ActionController.Instance.IgnoreMainButtonNextRelease();
          //  }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != 9 || collision.gameObject.layer != 10)
        {
            grounded = true;
        }

        // release the button after gliding makes next jump available
        if (grounded && !ActionController.Instance.MainButtonPressed)
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != 9 || collision.gameObject.layer != 10)
        {
            grounded = false;
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 9 || other.gameObject.layer != 10)
            grounded = true;

        if (!ActionController.Instance.MainButtonPressed)
            jumping = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9 || other.gameObject.layer != 10)
        {
            grounded = false;
            jumping = true;
        }
    }
    */

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

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!grounded && falling)
    //    {
    //        crow.transform.position = crowStandardTransform.position;
    //        anim.SetTrigger("jump_end");
    //    }
        
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    grounded = false;
    //}

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
        return Physics.Raycast(groundCheckTransform.position, -transform.up, groundCheckDistance, ignorePlayer);
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