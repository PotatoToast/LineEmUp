using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Coin coin;

    public void PlaceCoin(){
        // if (GameManager.Instance.currentPlayer == player.playerNumber){
        //     coin.playerNumber = player.playerNumber;
        //     Instantiate(coin, gameObject.transform);
        //     GameManager.Instance.SwitchPlayer();
        // }
        int currentPlayer = GameManager.Instance.currentPlayer;
        coin.playerNumber = currentPlayer;
        Instantiate(coin, gameObject.transform);
        GameManager.Instance.SwitchPlayer();
    }
}
