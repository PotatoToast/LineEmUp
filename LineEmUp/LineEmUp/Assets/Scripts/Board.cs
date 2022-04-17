using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Coin[,] grid;

    private SelectCoin input;

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
        //Not finished XD. The input is suppose to check which coin you click to remove. 
        //It checks if there is a coin adjacent to the one you choose and uses the remove function. 
        if ( grid[row, col + 1] != null && input.checkObjectClicked != null ) 
        {
            RemoveCoin(row, col + 1);
        }
        else if ( grid[row + 1, col] != null && input.checkObjectClicked != null )
        {
            RemoveCoin(row + 1, col);
        }
        else if ( grid[row - 1, col] != null && input.checkObjectClicked != null )
        {
            RemoveCoin(row - 1, col);
        }
    }

    public void ProtectAdjacent()
    {
        
    }

    public void PushRow()
    {

    }
    #endregion
}
