using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSelector : MonoBehaviour
{
    public int range; // half width of board
    public float shift = 3f; // how far over the selector moves
    private GameObject selector;
    public CoinSpawner coinSpawner;
    public CoinDropper dropper;


    [SerializeField] private List<GameObject> coinSpawnerLocations;
    private int spawnLocation = 0;

    // Start is called before the first frame update
    void Start()
    {
        dropper.InitializePositions();   
    }
    

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("a") || Input.GetKeyDown("left")) && spawnLocation > 0){
            //transform.position += new Vector3(-shift, 0, 0);
            spawnLocation -= 1;
            Debug.Log("Spawner Left");
        }
        //if a or left arrow move selector left
        
        else if ((Input.GetKeyDown("d") || Input.GetKeyDown("right")) && spawnLocation < 8){
            //transform.position += new Vector3(shift, 0, 0);
            spawnLocation += 1;
            Debug.Log("Spawner Right");
        }
        //if d or right arrow move right
        

        if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("enter")) {
            //Here is where the coin span function goes
            var loc = coinSpawnerLocations[spawnLocation].transform;
            coinSpawner.PlaceCoin(loc.position);
            Debug.Log(spawnLocation);
            //return;
        }
    }
}
