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

    public TextMeshProUGUI playerText;

    public GameObject coinMaster;

    private void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new Board(numRows, numCols);

        Coin[,] grid = board.GetGrid();

        int playerNumber = 1;
        int otherNumber = 2;
       

        //start testing
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

        board.PrintGrid();
        //end testing

        //playerText.text = "Player: " + currentPlayer;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (board.PlaceCoinInCol(col, players[currentPlayer-1].playerCoin)){
            SwitchPlayer();
        }
        board.PrintGrid();
        if (board.CheckWin(1)) Debug.Log("Player 1 wins");
        else if (board.CheckWin(2)) Debug.Log("Player 2 wins");
    }

    public void SwitchPlayer(){
        if (currentPlayer == 1){
            currentPlayer = 2;
        }
        else if(currentPlayer == 2){
            currentPlayer = 1;
        }
        playerText.text = "Player: " + currentPlayer;
    }
}
