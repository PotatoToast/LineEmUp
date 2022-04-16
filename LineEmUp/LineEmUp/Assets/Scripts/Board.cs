using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private GameObject[,] board;


    public Board(int row, int col)
    { 
        board = new GameObject[row, col];
    }


    public GameObject[,] GetBoard()
    {
        return board;
    }

    public void PlaceCoinInCol(int col, GameObject coin)
    {
        
    }

    /// <summary>
    /// This function does this
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void PlaceCoin(int row, int col)
    {
        //Somebody write function here
    }

    public void RemoveCoin(int row, int col)
    {
        //Write remove coin function here
    }
}
