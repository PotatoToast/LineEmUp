using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region SingletonCode
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public void CreateSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion 

    public Board board;

    public int numRows = 7;
    public int numCols = 9;

    public int currentPlayer = 1;
    public List<Player> players;

    public GameObject coinMaster;

    [SerializeField] private CanvasManager canvasManager;

    [SerializeField] private GameObject DestroyCoinPSEffect;

    private void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new Board(numRows, numCols);
        board.PrintGrid();
        canvasManager.UpdateCurrentPlayerText(currentPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            DestroyCoin(4, 0);
        }
    }

    private void TestingFunction()
    {
        //start testing
        
        Coin[,] grid = board.GetGrid();
        int playerNumber = 1;
        int otherNumber = 2;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (i % 3 == 0 || j % 5 == 0)
                {
                    continue;
                }
                //create new gameObject
                GameObject tempObj = new GameObject();
                tempObj.transform.position = new Vector3(i, j, 0);
                tempObj.transform.parent = coinMaster.transform;

                //Add coin component
                var tempCoin = tempObj.AddComponent<Coin>();
                tempCoin.playerNumber = playerNumber;
                tempCoin.isProtected = true;

                //logic for player switching
                (playerNumber, otherNumber) = (otherNumber, playerNumber);
                board.PlaceCoin(i, j, tempCoin);
            }
        }

        board.PrintGrid();

        board.SettleAllColumns();

        board.PrintGrid();

        GameObject t = new GameObject();
        t.transform.position = new Vector3(0, 0, 0);

        //Add coin component
        var tc = t.AddComponent<Coin>();
        tc.playerNumber = 3;


        board.PushRow(6, 5, true, tc);

        board.DestroyAdjacent(3, 1, Direction.right);

        board.PrintGrid();
        //end testing
    }
    private void PrintBoard(){
        Coin[,] grid = board.GetGrid();
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            string rowOutput = "";
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] == null){
                    rowOutput += "0";
                }
                else{
                    rowOutput += grid[row, col].playerNumber;
                }
            }
            Debug.Log(rowOutput);
        }
    }

    public void PlaceCoinInCol(int col){
        board.PlaceCoinInCol(col, players[currentPlayer-1].playerCoin);
        PrintBoard();
        SwitchPlayer();
    }

    public void SwitchPlayer(){

        currentPlayer++;
        if (currentPlayer > players.Count)
        {
            currentPlayer = 1;
        }

        canvasManager.UpdateCurrentPlayerText(currentPlayer);
        CheckIfEitherPlayerWin();
    }

    //Function that places coin in both physical and backend boards
    public void PlaceCoin(GameObject coin, Vector3 pos, int colNum)
    {
        GameObject temp = players[currentPlayer - 1].coinPrefab;

        //Creates backend version
        Coin myCoin = temp.GetComponent<Coin>();
        myCoin.playerNumber = currentPlayer;

        //if the board allows, create a physical copy)
        if (board.PlaceCoinInCol(colNum, myCoin))
        {
            //Creates physical version of coin
            GameObject obj = Instantiate(temp, pos, Quaternion.Euler(0, -90, 0));
            SwitchPlayer();
            board.PrintGrid();
        }
        else
        {
            Debug.Log("Invalid action");
            //Play invalid action noise here
        }
    }

    public void CheckIfEitherPlayerWin()
    {
        bool playerOneWins = board.CheckWin(1);
        bool playerTwoWins = board.CheckWin(2);

        if (playerOneWins && playerTwoWins)
        {
            canvasManager.DisplayGameResults(-1);
        }
        else if (playerOneWins)
        {
            canvasManager.DisplayGameResults(1);
        }
        else if(playerTwoWins)
        {
            canvasManager.DisplayGameResults(2);
        }
    }

    public void DestroyCoin(int row, int col)
    {
        var grid = board.GetGrid();
        Coin myCoin = grid[row, col];
        GameObject coinObj = myCoin.gameObject;
        GameObject psEffect = Instantiate(DestroyCoinPSEffect, coinObj.transform.position, Quaternion.Euler(0, 0, -60));

        psEffect.GetComponent<ParticleSystem>().Emit(1);
        Destroy(coinObj, 0.76f);
        Destroy(psEffect, 3f);
        board.RemoveCoin(row, col);
    }


}
