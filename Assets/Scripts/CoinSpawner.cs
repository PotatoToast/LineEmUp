using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;
    public GameObject destroyCoin;
    public GameObject protectCoin;
    public GameObject moveRowCoin;

    public int destroyCoinCost;
    public int protectCoinCost;
    public int pushCoinCost;

    private Coin.CoinType coinToSpawn;

    public void PlaceCoin(Vector3 pos, int colNum)
    {
        Player currPlayer = GameManager.Instance.GetCurrentPlayer();
        GameObject spawnCoin;
        switch(coinToSpawn){
            case Coin.CoinType.Default:
                spawnCoin = coin;
                break;
            case Coin.CoinType.Destroy:
                spawnCoin = destroyCoin;
                currPlayer.IncreaseAbilityPoints(-destroyCoinCost);
                break;
            case Coin.CoinType.Protect:
                spawnCoin = protectCoin;
                currPlayer.IncreaseAbilityPoints(-protectCoinCost);
                break;
            case Coin.CoinType.Push:
                spawnCoin = moveRowCoin;
                currPlayer.IncreaseAbilityPoints(-pushCoinCost);
                break;
            default:
                spawnCoin = coin;
                break;
        }
        GameManager.Instance.PlaceCoin(spawnCoin, pos, colNum);
        LoadDefaultCoin();
    }

    public void LoadDefaultCoin(){
        coinToSpawn = Coin.CoinType.Default;
        CanvasManager.Instance.UpdateCurrentCoinText(Coin.CoinType.Default);
    }

    public void LoadDestroyCoin(){
        Player currPlayer = GameManager.Instance.GetCurrentPlayer();
        if (currPlayer.abilityPoints >= destroyCoinCost){
            coinToSpawn = Coin.CoinType.Destroy;
            CanvasManager.Instance.UpdateCurrentCoinText(Coin.CoinType.Destroy);
        }
        else{
            Debug.Log("Not enough points");
        }
    }

    public void LoadProtectCoin(){
        Player currPlayer = GameManager.Instance.GetCurrentPlayer();
        if (currPlayer.abilityPoints >= protectCoinCost){
            coinToSpawn = Coin.CoinType.Protect;
            CanvasManager.Instance.UpdateCurrentCoinText(Coin.CoinType.Protect);
        }
        else{
            Debug.Log("Not enough points");
        }
    }

    public void LoadPushCoin(){
        Player currPlayer = GameManager.Instance.GetCurrentPlayer();
        if (currPlayer.abilityPoints >= pushCoinCost){
            coinToSpawn = Coin.CoinType.Push;
            CanvasManager.Instance.UpdateCurrentCoinText(Coin.CoinType.Push);
        }
        else{
            Debug.Log("Not enough points");
        }
    }
}
