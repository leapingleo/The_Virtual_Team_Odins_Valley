using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform resetPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        player.transform.position = resetPoint.position;
    }
}
