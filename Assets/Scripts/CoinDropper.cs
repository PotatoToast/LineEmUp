using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    private Vector3[,] coinPositions;
    public float deltaX;    // Distance between each col
    public float deltaY;    // Distance between each row
    public Vector3 startPos;// Coin position at top left of board

    public void InitializePositions()
    {
        int numRows = GameManager.Instance.numRows;
        int numCols = GameManager.Instance.numCols;

        coinPositions = new Vector3[numRows, numCols];
        float xPos = startPos.x;
        float yPos = startPos.y;
        float zPos = startPos.z;
        Vector3 newPos;

        for (int i=0; i < numRows; i++)
        {
            for (int j=0; j < numCols; j++)
            {
                newPos = new Vector3(xPos + (deltaX * j), yPos - (deltaY * i), zPos);
                coinPositions[i, j] = newPos;
            }
        }
    }

    public void DropCoin(int col){

    }
}
