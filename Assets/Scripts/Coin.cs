using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int playerNumber = 0;

    public bool isProtected = false;
    public Material blueMaterial;
    public Material yellowMaterial;

    public bool isSpecialCoin;
    public bool isPush;

    public void ChangePlayerNumber(int _playerNumber)
    {
        var meshrenderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        playerNumber = _playerNumber;
        if (playerNumber == 1)
        {
            foreach (MeshRenderer item in meshrenderers)
            {
                item.material = yellowMaterial;
            }
        }
        else if (playerNumber == 2)
        {
            
            foreach (MeshRenderer item in meshrenderers)
            {
                item.material = blueMaterial;
            }
        }
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    public void ButtonDestroyCoinUI(int input)
    {
        GameManager gm = GameObject.Find("GameManager (1)").GetComponent<GameManager>();
        gm.ButtonDestroyCoin(input);
    }

    public void ButtonProtectCoinUI(int input)
    {
        GameManager gm = GameObject.Find("GameManager (1)").GetComponent<GameManager>();
        gm.ButtonProtectCoin(input);
    }

    public void ButtonPushCoinUI(int input)
    {
        GameManager gm = GameObject.Find("GameManager (1)").GetComponent<GameManager>();
        gm.ButtonPushCoin(input);
    }
}
