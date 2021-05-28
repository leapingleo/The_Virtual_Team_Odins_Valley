using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinPartTeleport : MonoBehaviour
{
    public Vector3 originalPos;
    public Vector3 originalRotation;
    public float originalScale;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("DisappearReappear");
            StartCoroutine(DisReappear());
            GetComponent<SineMovement>().enabled = false;
        }
            
    }

    IEnumerator DisReappear()
    {
        yield return new WaitForSeconds(1.5f);
       
        transform.position = originalPos;
        transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);
    }
}
