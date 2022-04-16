using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Awake()
    {
        CreateSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new Board(numRows, numCols);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
