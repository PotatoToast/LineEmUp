using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI player1Points;
    [SerializeField] private TextMeshProUGUI player2Points;


    public TextMeshProUGUI playerText;
    public TextMeshProUGUI winText;

    // Start is called before the first frame update
    void Start()
    {
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
