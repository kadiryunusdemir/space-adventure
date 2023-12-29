using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Player : MonoBehaviour
{
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        scoreSO.DecreaseInt(scoreSO.Number);
        if(healthSO.Number > 0)
        {
            healthSO.DecreaseInt(healthSO.Number - 4);
        }
        else
        {
            healthSO.IncreaseInt(4);
        }
    }
    private void Update()
    {
        if (scoreSO.Number == 5)
        {
            gameManager.ChangeGameState(Enums.GameState.Win);
        }
        if (healthSO.Number == 0)
        {
            gameManager.ChangeGameState(Enums.GameState.Lose);
        }
    }
}
