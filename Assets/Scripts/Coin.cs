using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int playerNumber = 0;

    public bool isProtected = false;
    public Material blueMaterial;
    public Material yellowMaterial;

    public void ChangePlayerNumber(int _playerNumber)
    {
        var meshrenderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        playerNumber = _playerNumber;
        if (playerNumber == 1)
        {
            foreach (MeshRenderer item in meshrenderers)
            {
                item.material = blueMaterial;
            }
        }
        else if (playerNumber == 2)
        {
            
            foreach (MeshRenderer item in meshrenderers)
            {
                item.material = yellowMaterial;
            }
        }
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
}
