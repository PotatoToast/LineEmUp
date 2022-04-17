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

    public void ProtectAdjacent(int row, int col)
    {
        grid[row, col].isProtected = true;
    }

    public void PushRow(int row, int col, bool left)
    {
        PlaceCoin(row, col);

        if (!left)
        {
            int upperBound = 9;
            for (int horizontalPos = col; horizontalPos < 9; horizontalPos++)
            {
                if (grid[row, horizontalPos].isProtected) upperBound = horizontalPos;
                grid[row, horizontalPos].isProtected = false;
            }

            for (int horizontalPos = upperBound - 1; horizontalPos > col; horizontalPos++)
            {
                grid[row, horizontalPos] = grid[row, horizontalPos - 1];
            }
            if (col != 8)
            {
                grid[row, col + 1] = null;
                //add falling
            }


        }

        else
        {
            int lowerBound = 0;
            for (int horizontalPos = col; lowerBound > 0; horizontalPos++)
            {
                if (grid[row, horizontalPos].isProtected) lowerBound = horizontalPos;
                grid[row, horizontalPos].isProtected = false;
            }

            for (int horizontalPos = lowerBound; horizontalPos < col; horizontalPos++)
            {
                grid[row, horizontalPos] = grid[row, horizontalPos + 1];
            }
            if (col != 0)
            {
                grid[row, col - 1] = null;
            }
        }
    }
    #endregion
}
