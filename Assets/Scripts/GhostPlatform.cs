using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] float dissapearTime = 3;

    Animator animator;

    [SerializeField] bool canReset;
    [SerializeField] float timeReset;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("DissapearTime", 1 / dissapearTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
            animator.SetBool("Trigger", true);
    }

    public void TriggerReset()
    {
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(timeReset);
        animator.SetBool("Trigger", false);
    }
}
