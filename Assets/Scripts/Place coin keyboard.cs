using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboardselector : MonoBehaviour
{
    public GameObject selector;
    
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(new selector, Vector3 (0, 10.5, 0) );
        //create boundaries for icon
        //place icon at position 0
        //creates the selector icon
        //make icon have no collision

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey("a") | Input.GetKey("left")) & (transform.position.y >= -2)){
            transform.position += new Vector3(-1, 0, 0);
        }
        //if a or left arrow move selector left

        if ((Input.GetKey("d") | Input.GetKey("right")) & (transform.position.y <= 2)){
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
