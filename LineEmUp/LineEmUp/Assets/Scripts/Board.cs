using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Coin[,] board;

    public Board(int row, int col)
    { 
        board = new Coin[row, col];
    }


    public Coin[,] GetBoard()
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

    #region Abilities

    public void DestroyAdjacent()
    { 

    }

    public void ProtectAdjacent()
    {
        
    }

    public void PushRow()
    {

    }
    #endregion
}
