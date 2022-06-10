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

    [SerializeField] private CanvasManager canvasManager;

    [Header("ParticleEffects")]
    [SerializeField] private GameObject DestroyCoinPSEffect;

    [SerializeField] private List<GameObject> columnLocationsGO;
    [SerializeField] private KeyboardSelector keyboardSelector;
    [SerializeField] private AudioSource fallSFX;


    [HideInInspector] public List<Vector3> columnLocations;

    #region StateMachine Stuff
    public enum State
    {
        Wait, PlaceCoin, ChooseDirection, IdleForAnim, SwitchTurn
    }

    public State currState = State.Wait;

    private bool isPlacingCoin = false;
    private bool hasChosenDirection = false;
    private bool isIdlingForAnim = false;

    private float timeInAnim = 0f;

    #endregion

    GameObject recentCoin;
    Coin newCoin;
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

        foreach (var x in columnLocationsGO)
        {
            columnLocations.Add(x.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    #region Getters/Setters
    public List<Vector3> GetColumnLocations()
    {
        return columnLocations;
    }
    #endregion


    #region For Testing
    public Player GetCurrentPlayer(){
        return players[currentPlayer - 1];
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
                tempCoin.ProtectCoin();

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
    #endregion

    void CheckState()
    {
        switch (currState)
        {
            case State.Wait:
                keyboardSelector.CheckForPlace();
                if (isPlacingCoin)
                {
                    currState = State.PlaceCoin;
                    isPlacingCoin = false;
                }
                break;
            case State.PlaceCoin:
                if (recentCoin != null)
                {
                    if (recentCoin.GetComponent<Rigidbody>().velocity.y > -0.05)
                    {
                        if (recentCoin.GetComponent<Coin>().isSpecialCoin)
                        {
                            Coin[,] grid = board.GetGrid();
                            hasChosenDirection = false;
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
                            currState = State.ChooseDirection;
                        }
                        else
                        {
                            fallSFX.Play();
                            currState = State.SwitchTurn;
                        }
                    }
                    /*
                    columnSelector.transform.GetChild(0).gameObject.SetActive(false);
                    columnSelector.transform.GetChild(1).gameObject.SetActive(false);
                    columnSelector.transform.GetChild(2).gameObject.SetActive(false);
                    */

                    //move column selector ui and only show available buttons
                    //columnSelector.transform.position = new Vector3(1, newCoin.transform.position.y,newCoin.transform.position.z);
                }

                break;
            case State.ChooseDirection:
                if (hasChosenDirection)
                {
                    currState = State.IdleForAnim;
                }
                break;
            case State.IdleForAnim:
                if (timeInAnim <= 0)
                {
                    currState = State.SwitchTurn;
                }
                else
                {
                    timeInAnim -= Time.deltaTime;
                }
                break;
            case State.SwitchTurn:
                SwitchPlayer();
                currState = State.Wait;
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
    }

    //Function that places coin in both physical and backend boards

   
    public void PlaceCoin(GameObject coin, Vector3 pos, int colNum)
    {
        GameObject temp = coin;

        //if the board allows, create a physical copy)
        if (board.canPlaceInColumn(colNum))
        {
            //Creates physical version of coin
            recentCoin = Instantiate(temp, pos, Quaternion.Euler(0, 90, 0));
            newCoin = recentCoin.GetComponent<Coin>();
            newCoin.ChangePlayerNumber(currentPlayer);

            recentCoin.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0);

            //Creates backend version
            board.PlaceCoinInCol(colNum, newCoin);

            isPlacingCoin = true;
            //SwitchPlayer();
            board.PrintGrid();
        }
        else
        {
            Debug.Log("Invalid action");
            return;
            //Play invalid action noise here
        }

        isPlacingCoin = true;

        /*
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
        */
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

    #region ButtonFunctions
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
        hasChosenDirection = true;
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
        hasChosenDirection = true;
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
        hasChosenDirection = true;
        canUseAbility = false;
    }

    #endregion

    #region Abilities
    public void DestroyCoin(int row, int col)
    {
        var grid = board.GetGrid();
        Coin myCoin = grid[row, col];
        GameObject coinObj = myCoin.gameObject;
        GameObject psEffect = Instantiate(DestroyCoinPSEffect, coinObj.transform.position, Quaternion.Euler(0, 0, 60));


        //check to see if the targeted coin is protected
        if (grid[row, col].isProtected)
        {
            grid[row, col].RemoveProtection();
            Destroy(psEffect, 3f);
            return;
        }

        //psEffect.GetComponent<ParticleSystem>().Emit(1);
        Destroy(coinObj, 0.76f);
        Destroy(psEffect, 3f);
        board.RemoveCoin(row, col);
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

        board.PushRow(row, col, pushLeft, coin);

        /*
        if (pushLeft) 
        {
            for (int i = 0; i < col; i++)
            {
                if (grid[row,i] != null && grid[row, i + 1] != null)
                {
                    grid[row, i].gameObject.transform.position = grid[row, i].gameObject.transform.position + new Vector3(0, 0, -1.65f);
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

            for (int i = numCols - 1; i > col; i--)
            {
                if (grid[row, i] != null && grid[row, i - 1] != null)
                {
                    grid[row, i].gameObject.transform.position = grid[row, i].gameObject.transform.position + new Vector3(0, 0, +1.65f);
                }
            }

            if (grid[row, numCols - 1] != null)
            {
                Destroy(grid[row, numCols - 1].gameObject);
            }

            board.PushRow(row, col, !pushLeft, coin);
        }
        */

        board.PrintGrid();
    }
    #endregion

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
