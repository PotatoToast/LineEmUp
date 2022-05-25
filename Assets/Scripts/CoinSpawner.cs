using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;

    public void PlaceCoin(Vector3 pos, int colNum)
    {
        GameManager.Instance.PlaceCoin(coin, pos, colNum);
    }
}
