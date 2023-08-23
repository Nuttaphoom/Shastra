using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnCollision : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject replace;
    private void OnCollisionEnter(Collision hit)
    {
       
        GameObject.Instantiate(replace, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
