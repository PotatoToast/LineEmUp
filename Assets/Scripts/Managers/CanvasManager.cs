using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager _instance;

    public static CanvasManager Instance { get { return _instance; } }

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

    [SerializeField] private TextMeshProUGUI player1Points;
    [SerializeField] private TextMeshProUGUI player2Points;
    [SerializeField] private TextMeshProUGUI currentCoinText;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI winText;

    // Start is called before the first frame update
    void Start()
    {
        CreateSingleton();
        winText.gameObject.SetActive(false);
    }

    public void UpdateCurrentPlayerText(int playerNum)
    {
        playerText.text = "Player: " + playerNum;
    }

    public void UpdatePlayerAbilityPoints(int playerNum, int newPoints){
        switch(playerNum){
            case 1:
                player1Points.text = "Player1: " + newPoints;
                break;
            case 2:
                player2Points.text = "Player2: " + newPoints;
                break;
            default:
                Debug.Log("player " + playerNum + " not found");
                break;
        }
    }

    public void UpdateCurrentCoinText(Coin.CoinType type){
        currentCoinText.text = type.ToString();
    }

    public void DisplayGameResults(int playerNum)
    {
        if (playerNum == -1)
        {
            winText.text = "Draw!";
            return;
        }
        winText.gameObject.SetActive(true);
        winText.text = "Player: " + playerNum + " Wins!";
    }


}
