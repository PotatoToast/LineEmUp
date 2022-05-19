using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public enum Direction { left, right, up, down };
public class Board
{
    private Coin[,] grid;
    private int numRows;
    private int numCols;

    public Board(int row, int col)
    { 
        grid = new Coin[row, col];
        numRows = row;
        numCols = col;
    }


    public Coin[,] GetGrid()
    {
        return grid;
    }

    /// <summary>
    /// Prints the grid
    /// </summary>
    /// Last updated : 4/17/2022
    /// Last tested : 4/17/2022
    /// Question? Ask (recent updater) : Calvin
    /// Functions correctly? : Yes
    public void PrintGrid()
    {
        //Open a text document and past the debug result. Will make reading easier
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                sb.Append(" ");
                if (grid[i, j] == null)
                {
                    sb.Append("0 ");
                    // sb.Append(string.Format("{0, 3}", '0'));
                }
                else
                {
                    sb.Append(grid[i, j].playerNumber);
                    if (grid[i, j].isProtected){
                        sb.Append("p");
                        // sb.Append(string.Format("{0, 3}", grid[i, j].playerNumber + "p"));
                        // sb.Append(string.Format("{0, 3}", 'p'));
                    }
                    else{
                        sb.Append(" ");
                        // sb.Append(string.Format("{0, 3}", grid[i, j].playerNumber));
                    }
                }
                sb.Append(" | ");
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Places a coin at the specified column. The coin will fall to the lowest row possible
    /// </summary>
    /// <param name="col"></param>
    /// <param name="coin"></param>
    /// Last updated : 4/17/2022
    /// Last tested : 4/17/2022
    /// Question? Ask (recent updater) : Calvin
    /// Functions correctly? : Yes
    public void PlaceCoinInCol(int col, Coin coin)
    {
        for (int i=0; i < numRows - 1; i++){
            if (grid[i+1,col] != null){
                PlaceCoin(i, col, coin);
                return;
            }
        }
        PlaceCoin(numRows-1, col, coin);
    }

    /// <summary>
    /// Places a coin in the row and column specified
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

    /// <summary>
    /// Moves all coins down the column
    /// </summary>
    /// Last updated : 4/17/2022
    /// Last tested : 4/17/2022
    /// Question? Ask (recent updater) : Calvin
    /// Functions correctly? : I think so
    public void SettleColumn(int col){
        int currComplete = numRows - 1;
        for (int i = numRows - 1; i > 0; i--)
        {
            if (grid[i, col] != null)
            {
                continue;   //Check if spot is already occupied, if so move up
            }

            int temp = 1;
            while (i - temp > 0 && grid[i - temp, col] == null)
            {
                temp++;
            }
            grid[i, col] = grid[i - temp, col];
            grid[i - temp, col] = null;
            /*
            if (grid[i-1, col] != null)         // Check everything above
            {                                   // If there is a coin above
                grid[i, col] = grid[i-1, col];  // Move it down
                grid[i-1, col] = null;          
            }
            */
        }
    }

    public void SettleAllColumns()
    {
        for (int i = 0; i < numCols; i++)
        {
            SettleColumn(i);
        }
    }

    #region Abilities

    public void DestroyAdjacent(int row, int col, Direction dir)
    {
        switch (dir)
        {
            case Direction.left:
                if (col - 1 < 0) return;
                RemoveCoin(row, col - 1);
                break;
            case Direction.right:
                if (col + 1 >= numCols) return;
                RemoveCoin(row, col + 1);
                break;
            case Direction.up:
                if (row - 1 < 0) return;
                RemoveCoin(row - 1, col);
                break;
            case Direction.down:
                if (row + 1 >= numRows) return;
                RemoveCoin(row + 1, col);
                break;
        }
    }

    public void ProtectAdjacent(int row, int col)
    {
        grid[row, col].isProtected = true;
    }

    /// <summary>
    /// Pushs the row specified.
    /// </summary>
    /// Last updated : 4/17/2022
    /// Last tested : 4/17/2022
    /// Question? Ask (recent updater) : Calvin
    /// Functions correctly? : Some simple cases worked. Probably needs more testing
    public void PushRow(int row, int col, bool left, Coin coin)
    {
        PlaceCoinInCol(col, coin);

        if (!left)
        {
            int upperBound = 9;
            for (int horizontalPos = col; horizontalPos < 9; horizontalPos++)
            {
                if (grid[row, horizontalPos] == null) continue;
                if (grid[row, horizontalPos].isProtected)
                {
                    upperBound = horizontalPos;
                    grid[row, horizontalPos].isProtected = false;
                    break;
                }
            }

            for (int horizontalPos = upperBound - 1; horizontalPos > col; horizontalPos--)
            {
                if (horizontalPos == col + 1)
                {
                    grid[row, horizontalPos] = null;
                }
                else
                {
                    grid[row, horizontalPos] = grid[row, horizontalPos - 1];
                }
                SettleColumn(horizontalPos);
            }
        }
        else
        {
            int lowerBound = -1;
            for (int horizontalPos = col; horizontalPos >= 0; horizontalPos--)
            {
                if (grid[row, horizontalPos] == null) continue;
                if (grid[row, horizontalPos].isProtected)
                {
                    lowerBound = horizontalPos;
                    grid[row, horizontalPos].isProtected = false;
                }
                break;
                
            }

            for (int horizontalPos = lowerBound + 1; horizontalPos < col; horizontalPos++)
            {
                if (horizontalPos == col - 1)
                {
                    grid[row, horizontalPos] = null;
                }
                else
                {
                    grid[row, horizontalPos] = grid[row, horizontalPos + 1];
                }
                SettleColumn(horizontalPos);
            }
            /*
            if (col != 0)
            {
                grid[row, col - 1] = null;
                SettleColumn(col);
            }
            */
        }
    }
    #endregion
}
