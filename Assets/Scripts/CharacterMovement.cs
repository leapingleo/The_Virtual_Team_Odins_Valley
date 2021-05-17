using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovement : MonoBehaviour
{
    /*
     * Called when velocity < 0, as in rising.
     */
    public float fallingMultiplier;

    /*
     * Called when the character is falling, as in velocity > 0
     */
    public float lowJumpMultiplier;
    
 
    public float jumpForce;
    public Rigidbody rigidBody;
    public float glideDistanceFactor;
    public float runTrigger;
    public float glideSpeed;
    public float groundCheckDistance;
    public float yiyiGravityModifier;
    public PlayerGrounded playerGrounded;
    public float moveSpeed;
    public float attackSpeed;
    public CustomGravity customGravity;
    public float jumpActivateTime;
    private float jumpTimer;
    public Animator anim;
    public GameObject leftAxe;
    public GameObject rightAxe;
    public float glideForceFactor;


    public Transform crowGlideTransform;
    public Transform crowStandardTransform;
    public GameObject crow;

    private float glideFactor;
    private float speed;
    private Vector3 moveDir;
    private bool grounded;
    private bool jumpRequest = false;
    private bool holdingDownMainButton = false;
    private bool holdingDownGlideButton = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.useGravity = false;
        customGravity.SetRigidBody(rigidBody);
    }

    private void Update()
    {

        if (ActionController.Instance.MainButtonDown)
        {
            jumpTimer = jumpActivateTime;
        }

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;

            if (grounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("jump_mid_air"))
            {
                anim.SetTrigger("jump_start");
                jumpRequest = true;
                jumpTimer = 0;
            }
        }

        if (grounded && anim.GetCurrentAnimatorStateInfo(0).IsName("jump_mid_air"))
        {
            anim.SetTrigger("jump_end");
        }


        if (ActionController.Instance.MainButtonPressed)
        {
            if (!grounded && !holdingDownMainButton)
            {
                holdingDownGlideButton = true;
            }
            else
            {
                holdingDownMainButton = true;
            }
        }
        else
        {
            holdingDownMainButton = false;
            holdingDownGlideButton = false;
        }

        if (grounded && ActionController.Instance.SecondaryButtonPressed)
        {
            anim.SetTrigger("attack");
        }

        moveDir = new Vector3(ActionController.Instance.JoystickDirection.x, 0f, ActionController.Instance.JoystickDirection.z).normalized;

        if (moveDir != Vector3.zero)
        {
            if (grounded)
                anim.SetTrigger("walk");

            FacePosition(moveDir);
        }
        else
        {
            if (grounded)
                anim.SetTrigger("idle");
        }      
    }

    private void FixedUpdate()
    {
        if (jumpRequest && grounded)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
            rigidBody.AddForce(transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            
            jumpRequest = false;
            grounded = false;
        }
        else
        {
            grounded = playerGrounded.grounded;
        }

        if (!grounded)
        {
            if (holdingDownGlideButton)
            {
                glideFactor = glideForceFactor;
                crow.transform.position = crowGlideTransform.position;
            }
            else
            {
                glideFactor = 1;
                crow.transform.position = crowStandardTransform.position;
            }
                
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y * glideFactor, rigidBody.velocity.z);
        }
        if (grounded && crow.transform.position != crowStandardTransform.position)
        {
            crow.transform.position = crowStandardTransform.position;
        }

        if (rigidBody.velocity.y < 0)
        {
            customGravity.gravityScale = fallingMultiplier;
        }
        else if (rigidBody.velocity.y > 0 && !holdingDownMainButton)
        {
            customGravity.gravityScale = lowJumpMultiplier;
        }

        Move(moveDir);
        
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

        if (dir != Vector3.zero)
        { 
            rigidBody.MovePosition(transform.position + moveDir * speed * Time.fixedDeltaTime);
        }
    }

    public void FacePosition(Vector3 toFace)
    {
        transform.rotation = Quaternion.LookRotation(toFace);
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

    public void GetHitByEnemyAxe()
    {
        Debug.Log("hit");
        /*
        * THIS IS THE LOGIC FOR WHEN THE PLAYER GETS HIT!
        */

    }

}