using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Player : MonoBehaviour
{
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;

    private void CheckScore(int score)
    {
        if (score > 5)
        {
            GameManager.Instance.ChangeGameState(Enums.GameState.Win);
        }
    }
    
    private void CheckHealth(int health)
    {
        if (health <= 0)
        {
            GameManager.Instance.ChangeGameState(Enums.GameState.Lose);
        }
    }

    private void OnEnable()
    {
        scoreSO.IntChangeEvent.AddListener(CheckScore);
        healthSO.IntChangeEvent.AddListener(CheckHealth);
    }

    private void OnDisable()
    {
        scoreSO.IntChangeEvent.RemoveListener(CheckScore);
        healthSO.IntChangeEvent.RemoveListener(CheckHealth);
    }
}
