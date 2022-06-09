using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityForce;
    public Rigidbody rb;

    private bool isTouching = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouching){
            rb.AddForce(new Vector3(0, -gravityForce, 0));
        }
    }

    void OnCollisionEnter(Collision collision){
        isTouching = true;
    }
}
