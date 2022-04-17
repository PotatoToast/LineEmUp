using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Coin[,] grid;

    public Board(int row, int col)
    { 
        grid = new Coin[row, col];
    }


    public Coin[,] GetGrid()
    {
        return grid;
    }

    public void PlaceCoinInCol(int col, GameObject coin)
    {
        
    }

    /// <summary>
    /// This function does this
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void PlaceCoin(int row, int col, GameObject coin)
    {
        if (grid[row, col] != null)
        {
            Debug.LogError("Cannot place coin at this location. Coin already exists");
            return;
        }

        grid[row, col] = coin.GetComponent<Coin>();
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
