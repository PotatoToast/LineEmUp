using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSelector : MonoBehaviour
{
    public int range; // half width of board
    public float shift = 3f; // how far over the selector moves
    private GameObject selector;
    //public CoinSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetKeyDown("a") | Input.GetKeyDown("left")) & (transform.position.z >= -1.4-range)){
            transform.position += new Vector3(0, 0, -shift);
        }
        //if a or left arrow move selector left
        
        else if ((Input.GetKeyDown("d") | Input.GetKeyDown("right")) & (transform.position.z <= -1.4+range)){
            transform.position += new Vector3(0, 0, shift);
        }
        //if d or right arrow move right
        

        if (Input.GetKey("s") | Input.GetKey("down") | Input.GetKey("enter")) {
            //Here is where the coin span function goes
            return;
        }
    }
}
