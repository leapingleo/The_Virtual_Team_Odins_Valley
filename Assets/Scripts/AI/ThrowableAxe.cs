using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableAxe : MonoBehaviour
{
    private Vector3 playerPosition = Vector3.zero;
    public float moveSpeed;
    public float rotationSpeed;

    public Rigidbody rb;

    private void Update()
    {
        transform.RotateAround(transform.position, transform.right, rotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerPosition != Vector3.zero)
            rb.velocity = (playerPosition - transform.position).normalized * moveSpeed * Time.fixedDeltaTime;
    }

    public void SetPlayerPosition(Vector3 playerPosition)
    {
        this.playerPosition = playerPosition;
    }
}
