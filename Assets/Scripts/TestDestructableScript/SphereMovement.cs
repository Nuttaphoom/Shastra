using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 10.0f;
    float xInput,yInput;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Inputs()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        rb.AddForce(new Vector3(xInput, 0.0f, yInput) * moveSpeed);
        
    }
}
