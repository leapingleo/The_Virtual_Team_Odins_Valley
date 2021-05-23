using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovement : MonoBehaviour
{
    /*
     * Added this to ensure i dont get error, needs to be changed, sorry yiyi.
     */
    public float Lives;
    public float Collected;
    public Vector3 CheckPointPos;

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
    public PlayerNormal playerNormal;
    public float moveSpeed;
    public float attackSpeed;
    public CustomGravity customGravity;
    public float jumpActivateTime;
    private float jumpTimer;
    public Animator anim;
    public GameObject leftAxe;
    public GameObject rightAxe;

    public Transform rightLeg;
    public Transform leftLeg;
    public GameObject dustParticlePrefab;

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
    private bool usingGravity = false;
    public float inverseControlsThreshold;
    private ParticleSystem dustParticle;

    private bool movingUpsideDownNonInverse = false;
    


    // Start is called before the first frame update
    void Start()
    {
        dustParticle = Instantiate(dustParticlePrefab, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
        rigidBody.useGravity = false;
        customGravity.SetRigidBody(rigidBody);
    }

    private void Update()
    {

        /*
         * What can happen is that the player might jump off the gravity ledge 
         * and still be pushed by the velocity from that jumping ledge.
         */
        if (usingGravity && !playerNormal.UsingGravity)
        {
            rigidBody.velocity = Vector3.zero;
        }

        usingGravity = playerNormal.UsingGravity;


        if (ActionController.Instance.MainButtonDown)
        {
            ActionController.Instance.SetMainButtonDown(false);
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

        if (!grounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("jump_start")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("jump_mid_air")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("jump_end"))
        {
            anim.SetTrigger("fall");
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

        /*
         * If the player moves the character and enters upside down area then it should be inversed movement direction.
         * HOWEVER, if the character stops in an upside down area, then the controls are no longer inverted.
         */

        if (playerNormal.GroundNormal.y < -inverseControlsThreshold && moveDir == Vector3.zero)
        {
            movingUpsideDownNonInverse = true;
        }
        
        if (movingUpsideDownNonInverse && playerNormal.GroundNormal.y >= -inverseControlsThreshold && moveDir == Vector3.zero)
        {
            movingUpsideDownNonInverse = false;
        }

        if (moveDir != Vector3.zero)
        {
            if (grounded)
                anim.SetTrigger("walk");

            /*
             * Inverse the rotation as well.
             */
            if (playerNormal.GroundNormal.y < -inverseControlsThreshold)
            {
                if (movingUpsideDownNonInverse)
                {
                    FacePosition(new Vector3(moveDir.x, 0f, moveDir.z));
                }
                else
                {
                    FacePosition(new Vector3(-moveDir.x, 0f, moveDir.z));
                }
            }
            else
            {
                if (!movingUpsideDownNonInverse)
                {
                    FacePosition(playerNormal.AlignWithSurfaceRot * moveDir);
                }
                else
                {
                    FacePosition(playerNormal.AlignWithSurfaceRot * new Vector3(-moveDir.x, 0f, moveDir.z));
                }    
            }
            
        }
        else
        {
            if (grounded)
                anim.SetTrigger("idle");

            //transform.rotation = playerGrounded.AlignWithSurfaceRot;
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

            if (!usingGravity)
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y * glideFactor, rigidBody.velocity.z);
            else
                rigidBody.velocity = new Vector3(rigidBody.velocity.x * glideFactor, rigidBody.velocity.y * glideFactor, rigidBody.velocity.z * glideFactor);

        }

        if (grounded && crow.transform.position != crowStandardTransform.position)
        {
            crow.transform.position = crowStandardTransform.position;
        }

        Vector3 gravityVelocity = playerNormal.AlignWithSurfaceRot * rigidBody.velocity;

        if (gravityVelocity.y < 0)
        {
            customGravity.gravityScale = fallingMultiplier;
        }
        else if (gravityVelocity.y > 0 && !holdingDownMainButton)
        {
            customGravity.gravityScale = lowJumpMultiplier;
        }

        if (playerNormal.GroundNormal.y < -inverseControlsThreshold)
        {
            if (movingUpsideDownNonInverse)
            {
                Move(moveDir);
            }
            else
            {
                Move(new Vector3(-moveDir.x, 0f, moveDir.z));
            }
        }
        else
        {
            if (!movingUpsideDownNonInverse)
            {
                Move(playerNormal.AlignWithSurfaceRot * moveDir);
            }
            else
            {
                Move(playerNormal.AlignWithSurfaceRot * new Vector3(-moveDir.x, 0f, moveDir.z));
            }
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

        if (dir != Vector3.zero)
        { 
            rigidBody.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
        }
    }

    public void FacePosition(Vector3 toFace)
    {

        transform.rotation = Quaternion.LookRotation(toFace, playerNormal.GroundNormal );
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

    public void ParticleLeftLeg()
    {
        dustParticle.transform.position = leftLeg.transform.position;
        dustParticle.Play();
    }

    public void ParticleRightLeg()
    {
        dustParticle.transform.position = rightLeg.transform.position;
        dustParticle.Play();
    }

}