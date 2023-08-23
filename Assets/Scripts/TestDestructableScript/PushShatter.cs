using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushShatter : MonoBehaviour
{
    [SerializeField]
    private float forceMag;
    public Rigidbody rb;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


 
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision hit)
    {
        rb = hit.collider.attachedRigidbody;
        if(rb != null)
        {
            Vector3 direction = hit.transform.position - transform.position;
            direction.y = 0;
            rb.AddForce(direction.normalized * forceMag, ForceMode.Impulse);
        }

        
        
    }
}
