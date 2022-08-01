using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int coins;

    public void AddCoins(int count)
    {
        coins += count;
        Debug.Log(coins);
    }
}
