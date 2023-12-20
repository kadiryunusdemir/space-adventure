using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Object = UnityEngine.Object;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;

    private Action<Enums.GameState> gameStateAction;
    private Enums.GameState gameState;
    private Enums.LevelIndex levelIndex;
    private LevelData levelData;
    
    private void Start()
    {
        levelIndex = Enums.LevelIndex.Level1;
        ChangeGameState(Enums.GameState.MainMenu);
    }

    public void TestGameLose()
    {
        ChangeGameState(Enums.GameState.Lose);
    }
    
    public void TestGameWin()
    {
        ChangeGameState(Enums.GameState.Win);
    }

    private void ManageGame(Enums.GameState currentGameState)
    {
        Debug.Log("gamestate: "+ currentGameState);
        switch (currentGameState)
        {
            case Enums.GameState.Default:
                break;
            case Enums.GameState.MainMenu:
                break;
            case Enums.GameState.Starting:
                levelData = levelManager.GetLevelData(levelIndex);
                Debug.Log("test: " + levelData.Waves[0].WaveDensityPercentage);
                ChangeGameState(Enums.GameState.Playing);
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.Win:
                levelIndex++;
                break;
            case Enums.GameState.Lose:
                break;
            case Enums.GameState.Paused:
                Time.timeScale = 0;
                break;
            case Enums.GameState.GameEnded:
                // no more level
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeGameState(Enums.GameState newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            gameStateAction.Invoke(gameState);
        }
    }

    private void OnApplicationQuit()
    {
        if (gameState != Enums.GameState.MainMenu)
        {
            // save level
        }
    }

    private void OnEnable()
    {
        gameStateAction += ManageGame;
    }

    private void OnDisable()
    {
        gameStateAction -= ManageGame;
    }
}
