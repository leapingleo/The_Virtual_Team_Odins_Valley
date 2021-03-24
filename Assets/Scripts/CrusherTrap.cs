using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherTrap : MonoBehaviour
{
    Animator animator;

    [SerializeField] float waitTime;
    [SerializeField] [Range(0, 1)] float animationOffDuration;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("waitTime", 1 / waitTime);
        animator.Play("waitTime", -1, animationOffDuration);
    }
}
