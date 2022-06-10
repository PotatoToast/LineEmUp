using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSelector : MonoBehaviour
{
    public int range; // half width of board
    public float shift = 3f; // how far over the selector moves
    public Coin selector;
    public CoinSpawner coinSpawner;
    public CoinDropper dropper;

    [SerializeField] private List<GameObject> coinSpawnerLocations;
    private int spawnColNumber = 0;

    private Coin tempCoin; 
    // Start is called before the first frame update
    void Start()
    {
        dropper.InitializePositions();   
        var loc = coinSpawnerLocations[spawnColNumber].transform;
        Quaternion rot = selector.transform.rotation;
        selector = Instantiate(selector, loc.position, rot);
        selector.ChangePlayerNumber(GameManager.Instance.currentPlayer);
    }
    // Update is called once per frame
    void Update()
    {
        MoveSelector(); 
    }

    public void MoveSelector()
    {
        if ((Input.GetKeyDown("a") || Input.GetKeyDown("left")) && spawnColNumber > 0)
        {
            //transform.position += new Vector3(-shift, 0, 0);
            spawnColNumber -= 1;
            var loc = coinSpawnerLocations[spawnColNumber].transform;
            selector.transform.position = loc.position;
            Debug.Log("Spawner Left");
        }
        //if a or left arrow move selector left

        else if ((Input.GetKeyDown("d") || Input.GetKeyDown("right")) && spawnColNumber < 8)
        {
            //transform.position += new Vector3(shift, 0, 0);
            spawnColNumber += 1;
            var loc = coinSpawnerLocations[spawnColNumber].transform;
            selector.transform.position = loc.position;
            Debug.Log("Spawner Right");
        }
        //if d or right arrow move right
    }
    public void CheckForPlace()
    {
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("enter"))
        {
            //Here is where the coin span function goes
            var loc = coinSpawnerLocations[spawnColNumber].transform;
            coinSpawner.PlaceCoin(loc.position, spawnColNumber);
            selector.ChangePlayerNumber((GameManager.Instance.currentPlayer % 2) + 1);  // Gets oppposing player
            //return;
        }
    }
}
