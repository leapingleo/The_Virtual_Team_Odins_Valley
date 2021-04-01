using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    private float y;
    public int n = 9;
    public float timeFactor;
    public float amplitude;
    public int r;
    public Transform resetPoint;

    void Start()
    {
        y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = 0;
        for (int i = n; i > 0; i -= 2)
        {
            newY += amplitude * (Mathf.Sin(i * Time.time) * r / (i * Mathf.PI));
        }
        transform.position = new Vector3(transform.position.x, newY + y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sphere")) 
        {
            other.transform.position = resetPoint.position;
        }
    }
}
