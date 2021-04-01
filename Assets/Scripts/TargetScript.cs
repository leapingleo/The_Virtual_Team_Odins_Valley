using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Throwable"))
        {
            GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(DestroyObjects(3));
        }
    }
   
    IEnumerator DestroyObjects(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

}
