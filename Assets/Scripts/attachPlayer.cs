using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachPlayer : MonoBehaviour
{
    public Transform[] platform_points;
    public int point_number = 0;
    private Vector3 current_target;

    public float tolerance;
    public float speed;
    public float delay_time;

    private float delay_start;

    public bool automatic;

    private void Start()
    {
        if (platform_points.Length > 0)
        {
            current_target = platform_points[0].position;
        }
        tolerance = speed * Time.deltaTime;
    }

    private void Update()
    {
        if (transform.position != current_target)
        {
            platform_move();
        }
        else
        {
            update_target();
        }
    }

    void platform_move()
    {
        Vector3 heading = current_target - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            transform.position = current_target;
            delay_start = Time.time;
        }
    }

    void update_target()
    {
        if (automatic)
        {
            if (Time.time - delay_start > delay_time)
            {
                platform_next();
            }
        }
    }

    void platform_next()
    {
        point_number++;
        if (point_number >= platform_points.Length)
        {
            point_number = 0;
        }
        current_target = platform_points[point_number].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }



}
