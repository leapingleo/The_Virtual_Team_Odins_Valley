using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.transform.CompareTag("Tutorial"))
            {
                if (ActionController.Instance.LeftTriggerPressed || ActionController.Instance.RightTriggerPressed)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
                
        }
    }
}
