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

    public void PlaceCoinInCol(int col, Coin coin)
    {
        for (int i=0; i < grid.Length-1; i++){
            if (grid[i+1,col] != null){
                PlaceCoin(i, col, coin);
                return;
            }
        }
    }

    /// <summary>
    /// This function does this
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void PlaceCoin(int row, int col, Coin coin)
    {
        //Somebody write function here
        if (grid[row, col] != null){
            Debug.Log("Couldn't place coin at [" + row + "][" + col + "]");
        }
        else{
            grid[row,col] = coin;
        }
    }

    public void RemoveCoin(int row, int col)
    {
        //Write remove coin function here
        if (grid[row, col] != null){
            grid[row, col] = null;
            SettleColumn(col);
        }
    }

    public void SettleColumn(int col){
        for (int i=grid.Length; i > 0; i--){    // Check everything above
            if (grid[i-1, col] != null){        // If there is a coin above
                grid[i, col] = grid[i-1, col];  // Move it down
                grid[i-1, col] = null;          
            }
        }
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
