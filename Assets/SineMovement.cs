using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    private float yPos;
    private float amplitude, period;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        amplitude = Random.Range(0.01f, 0.05f);
        period = Random.Range(0.4f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float y = amplitude * Mathf.Sin(period * (Mathf.PI + Time.time)) + yPos;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
