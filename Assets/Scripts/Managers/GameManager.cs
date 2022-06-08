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

    public GameObject coinPrefab;
    public GameObject coinDestroyPrefab;
    public GameObject coinProtectPrefab;
    public GameObject coinPushPrefab;

    public GameObject coinMaster;

    public GameObject columnSelector;

    [SerializeField] private CanvasManager canvasManager;

    [SerializeField] private GameObject DestroyCoinPSEffect;

    [SerializeField] private List<GameObject> columnLocations;

    #region StateMachine Stuff
    private enum State
    {
        Wait, PlaceCoin, ChooseDirection, IdleForAnim
    }

    private State currState = State.Wait;

    #endregion

    GameObject recentCoin;
    Coin newCoin;
    bool canSelect = false;
    public bool canUseAbility = false; //waits until a player uses their ability before allowing more coins to be placed

    private void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new Board(numRows, numCols);
        board.PrintGrid();
        canvasManager.UpdatePlayerAbilityPoints(1, 0);
        canvasManager.UpdatePlayerAbilityPoints(2, 0);
        canvasManager.UpdateCurrentPlayerText(currentPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        Coin[,] grid = board.GetGrid();

        if (recentCoin != null && recentCoin.GetComponent<Coin>().isSpecialCoin && recentCoin.GetComponent<Rigidbody>().velocity.y > -0.05 && canSelect)
        {
            /*
            columnSelector.transform.GetChild(0).gameObject.SetActive(false);
            columnSelector.transform.GetChild(1).gameObject.SetActive(false);
            columnSelector.transform.GetChild(2).gameObject.SetActive(false);
            */

            //move column selector ui and only show available buttons
            //columnSelector.transform.position = new Vector3(1, newCoin.transform.position.y,newCoin.transform.position.z);

            if (FindGridColLocation() > 0 && grid[FindGridRowLocation(), FindGridColLocation() - 1] != null)
            {
                recentCoin.transform.GetChild(8).GetChild(0).gameObject.SetActive(true);
                //columnSelector.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (FindGridRowLocation() < numRows - 1 && grid[FindGridRowLocation() + 1, FindGridColLocation()] != null)
            {
                recentCoin.transform.GetChild(8).GetChild(1).gameObject.SetActive(true);
                //columnSelector.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (FindGridColLocation() < numCols - 1 && grid[FindGridRowLocation(), FindGridColLocation() + 1] != null)
            {
                recentCoin.transform.GetChild(8).GetChild(2).gameObject.SetActive(true);
                //columnSelector.transform.GetChild(2).gameObject.SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DestroyCoin(4, 0);
        }
    }

    private void TestingFunction()
    {
        //start testing

        Coin[,] grid = board.GetGrid();
        int playerNumber = 1;
        int otherNumber = 2;

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
                tempObj.transform.parent = coinMaster.transform;

                //Add coin component
                var tempCoin = tempObj.AddComponent<Coin>();
                tempCoin.ChangePlayerNumber(playerNumber);
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
        //tc.ChangePlayerNumber = 3;


        board.PushRow(6, 5, true, tc);

        board.DestroyAdjacent(3, 1, Direction.right);

        board.PrintGrid();
        //end testing
    }


    void DoStateMachine()
    {
        switch (currState)
        {
            case State.Wait:
                break;
            case State.PlaceCoin:
                break;
            case State.ChooseDirection:
                break;
            case State.IdleForAnim:
                break;
            default:
                break;
        }
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
                    rowOutput += grid[row, col].GetPlayerNumber();
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

        currentPlayer++;
        if (currentPlayer > players.Count)
        {
            currentPlayer = 1;
        }

        Player nextPlayer = players[currentPlayer - 1];
        nextPlayer.IncreaseAbilityPoints(1);

        canvasManager.UpdatePlayerAbilityPoints(currentPlayer, nextPlayer.abilityPoints);
        canvasManager.UpdateCurrentPlayerText(currentPlayer);
        CheckIfEitherPlayerWin();
        canSelect = true;
    }

    //Function that places coin in both physical and backend boards

   
    public void PlaceCoin(GameObject coin, Vector3 pos, int colNum)
    {
        GameObject temp = coinPrefab;

        //if the board allows, create a physical copy)
        if (board.canPlaceInColumn(colNum) && !canUseAbility)
        {
            //Creates physical version of coin
            
            recentCoin = Instantiate(temp, pos, Quaternion.Euler(0, -90, 0));
            newCoin = recentCoin.GetComponent<Coin>();
            newCoin.ChangePlayerNumber(currentPlayer);

            recentCoin.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0);

            //Creates backend version
            board.PlaceCoinInCol(colNum, newCoin);
            
            SwitchPlayer();
            board.PrintGrid();
        }
        else
        {
            Debug.Log("Invalid action");
            //Play invalid action noise here
        }
        if (temp.GetComponent<Coin>().isSpecialCoin == true) //if a player placed a special coin
        {
            var grid = board.GetGrid();

            //if the player is able to choose a direction for special coin ability

            if (FindGridColLocation() > 0 && grid[FindGridRowLocation(), FindGridColLocation() - 1] != null)
            {
                canUseAbility = true;
            }
            else if (!recentCoin.GetComponent<Coin>().isPush && FindGridRowLocation() < numRows - 1 && grid[FindGridRowLocation() + 1, FindGridColLocation()] != null) //only checks below if coin is destroy or protect
            {
                canUseAbility = true;
            }
            else if (FindGridColLocation() < numCols - 1 && grid[FindGridRowLocation(), FindGridColLocation() + 1] != null)
            {
                canUseAbility = true;
            }
        }
    }


    public void CheckIfEitherPlayerWin()
    {
        bool playerOneWins = board.CheckWin(1);
        bool playerTwoWins = board.CheckWin(2);

        if (playerOneWins && playerTwoWins)
        {
            canvasManager.DisplayGameResults(-1);
        }
        else if (playerOneWins)
        {
            canvasManager.DisplayGameResults(1);
        }
        else if(playerTwoWins)
        {
            canvasManager.DisplayGameResults(2);
        }
    }

    public void DestroyCoin(int row, int col)
    {
        var grid = board.GetGrid();
        Coin myCoin = grid[row, col];
        GameObject coinObj = myCoin.gameObject;
        GameObject psEffect = Instantiate(DestroyCoinPSEffect, coinObj.transform.position, Quaternion.Euler(0, 0, -60));


        //check to see if the targeted coin is protected
        if (grid[row, col].isProtected)
        {
            grid[row, col].isProtected = false;
            Destroy(psEffect, 3f);
            return;
        }

        //psEffect.GetComponent<ParticleSystem>().Emit(1);
        Destroy(coinObj, 0.76f);
        Destroy(psEffect, 3f);
        board.RemoveCoin(row, col);
    }
        
    public void ButtonDestroyCoin(int input)
    {
        //left
        if (input == -1) 
        {
            DestroyCoin(FindGridRowLocation(), FindGridColLocation() - 1);   
        }

        //bottom
        else if (input == 0) 
        {
            DestroyCoin(FindGridRowLocation() + 1, FindGridColLocation());
        }

        //right
        else if (input == 1) 
        {
            DestroyCoin(FindGridRowLocation(), FindGridColLocation() + 1);
        }
        recentCoin.transform.GetChild(8).GetChild(0).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(1).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(2).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(0).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(1).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(2).gameObject.SetActive(false);
        canSelect = false;
        canUseAbility = false;
    }

    public void ButtonProtectCoin(int input)
    {
        //left
        if (input == -1)
        {
            ProtectCoin(FindGridRowLocation(), FindGridColLocation() - 1);
        }

        //bottom
        else if (input == 0)
        {
            ProtectCoin(FindGridRowLocation() + 1, FindGridColLocation());
        }

        //right
        else if (input == 1)
        {
            ProtectCoin(FindGridRowLocation(), FindGridColLocation() + 1);
        }
        recentCoin.transform.GetChild(8).GetChild(0).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(1).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(2).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(0).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(1).gameObject.SetActive(false);
        //columnSelector.transform.GetChild(2).gameObject.SetActive(false);
        canSelect = false;
        canUseAbility = false;
    }

    public void ButtonPushCoin(int input)
    {
        if (input == -1)
        {
            PushCoin(FindGridRowLocation(), FindGridColLocation(), true, recentCoin.GetComponent<Coin>());
        }
        else if (input == 1)
        {
            PushCoin(FindGridRowLocation(), FindGridColLocation(), false, recentCoin.GetComponent<Coin>());
        }
        recentCoin.transform.GetChild(8).GetChild(0).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(1).gameObject.SetActive(false);
        recentCoin.transform.GetChild(8).GetChild(2).gameObject.SetActive(false);
        canSelect = false;
        canUseAbility = false;
    }

    public void ProtectCoin(int row, int col)
    {
        var grid = board.GetGrid();
        Coin myCoin = grid[row, col];
        GameObject coinObj = myCoin.gameObject;

        board.ProtectAdjacent(row, col);
    }

    public void PushCoin(int row, int col, bool pushLeft, Coin coin)
    {
        var grid = board.GetGrid();

        if (pushLeft) 
        {
            for (int i = 1; i < col; i++)
            {
                if (grid[row,i] != null && grid[row, i + 1] != null)
                {
                    Vector3 newPos = new Vector3(grid[row, i].gameObject.transform.position.x, grid[row, i].gameObject.transform.position.y, columnLocations[i - 1].transform.position.z);
                    grid[row, i].gameObject.transform.position = newPos;
                }
            }
            
            if (grid[row, 0] != null)
            {
                Destroy(grid[row, 0].gameObject);
            }

            board.PushRow(row, col, pushLeft, coin);
        }

        else if (!pushLeft)
        {

            for (int i = numCols - 2; i > col; i--)
            {
                if (grid[row, i] != null && grid[row, i - 1] != null)
                {
                    Vector3 newPos = new Vector3(grid[row, i].gameObject.transform.position.x, grid[row, i].gameObject.transform.position.y, columnLocations[i + 1].transform.position.z);
                    grid[row, i].gameObject.transform.position = newPos;
                }
            }

            if (grid[row, numCols - 1] != null)
            {
                Destroy(grid[row, numCols - 1].gameObject);
            }

            board.PushRow(row, col, !pushLeft, coin);
        }

        board.PrintGrid();
    }

    public int FindGridRowLocation()
    {
        Coin[,] grid = board.GetGrid();
        for (int i = 0; i < numRows; ++i)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (grid[i, j] == newCoin) return i;
            }
        }
        return -1;
    }

    public int FindGridColLocation()
    {
        Coin[,] grid = board.GetGrid();
        for (int i = 0; i < numRows; ++i)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (grid[i, j] == newCoin) return j;
            }
        }
        return -1;
    }

}
