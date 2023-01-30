using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Public Variables
    public float moveSpeed;
    public Rigidbody2D rb;

    // Private Variables
    private float horizontal, vertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }


    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal, vertical, 0).normalized;

        rb.MovePosition(transform.position + (movement * moveSpeed * Time.deltaTime));

    }
}
