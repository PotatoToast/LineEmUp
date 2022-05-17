using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSelector : MonoBehaviour
{
    public int range = 3;
    public float speed = 10f;
    public float maxVelocityChange = 10;
    private GameObject selector;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello"); 
        //Instantiate(new selector, Vector3 (0, 10.5, 0) );
        //create boundaries for icon
        //place icon at position 0
        //creates the selector icon
        //make icon have no collision
    }
    

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 targetVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        targetVelocity *= speed;

        Vector3 velocity = GetComponent<selector>(velocity);

        Vector3 velocityChange = (targetVelocity - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange.z = 0;
        selector.rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        */
        if ((Input.GetKeyDown("a") | Input.GetKeyDown("left")) & (transform.position.x >= -range)){
            transform.position += new Vector3(-1, 0, 0);
        }
        //if a or left arrow move selector left
        
        else if ((Input.GetKeyDown("d") | Input.GetKeyDown("right")) & (transform.position.x <= range)){
            transform.position += new Vector3(1, 0, 0);
        }
        //if d or right arrow move right
        

        if (Input.GetKey("s") | Input.GetKey("down") | Input.GetKey("enter")) {
            //myObject.GetComponent<CoinSpawner>().MyFunction();
            return;
        }
        //if enter or s or down arrow drop coin
        // then create coin script at position of icon     
    }
}
