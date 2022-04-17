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


    private void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new Board(numRows, numCols);

        Coin[,] grid = board.GetGrid();

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                // Debug.Log(grid[row, col]);
            }
        }
        playerText.text = "Player: " + currentPlayer;
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
        board.PlaceCoinInCol(col, players[currentPlayer-1].playerCoin);
        PrintBoard();
        SwitchPlayer();
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
