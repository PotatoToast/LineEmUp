using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
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
