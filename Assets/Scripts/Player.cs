using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerNumber = 1;
    public int abilityPoints = 0;
    public Coin playerCoin;

    public GameObject coinPrefab;
    
    public void IncreaseAbilityPoints(int increase){
        abilityPoints += increase;
    }

}
