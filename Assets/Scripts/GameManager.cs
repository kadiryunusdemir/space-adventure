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
    [SerializeField] private EnemySpawner enemySpawner;
    private Action<Enums.GameState> gameStateAction;
    private Enums.GameState gameState;
    private LevelData levelData;
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    
    private void Start()
    {
        // ChangeGameState(Enums.GameState.MainMenu);
        levelManager.IncreaseLevelIndex();
        ChangeGameState(Enums.GameState.Starting);
    }

    public void TestGameLose()
    {
        ChangeGameState(Enums.GameState.Lose);
    }
    
    public void TestGameWin()
    {
        ChangeGameState(Enums.GameState.Win);
    }

    private async void ManageGame(Enums.GameState currentGameState)
    {
        Debug.Log("gamestate: "+ currentGameState);
        switch (currentGameState)
        {
            case Enums.GameState.Default:
                break;
            case Enums.GameState.MainMenu:
                levelManager.OpenMainMenu();
                break;
            case Enums.GameState.Starting:
                levelData = levelManager.GetLevelData(levelManager.GetCurrentLevelIndex());
                await enemySpawner.CreateAsteroidShower(levelData);
                ChangeGameState(Enums.GameState.Playing);
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.Win:
                // TODO: win panel
                Debug.Log("currenLevel: " + levelManager.GetCurrentLevelIndex());
                PrepareNextLevel();
                if (levelManager.GetCurrentLevelIndex() < Enums.LevelIndex.Level3)
                {
                    levelManager.IncreaseLevelIndex();
                    ChangeGameState(Enums.GameState.Starting);
                }
                else
                {
                    // TODO:
                    ChangeGameState(Enums.GameState.MainMenu);
                }
                break;
            case Enums.GameState.Lose:
                // TODO: lose panel  
                PrepareNextLevel();
                gameState = Enums.GameState.Starting;
                break;
            case Enums.GameState.Paused:
                // TODO: pause panel  
                Time.timeScale = 0;
                break;
            case Enums.GameState.GameEnded:
                // TODO: game end panel  
                // no more level
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void CheckScore(int score)
    {
        if (gameState == Enums.GameState.Playing && score > 3)
        {
            ChangeGameState(Enums.GameState.Win);
        }
    }
    
    private void CheckHealth(int health)
    {
        if (gameState == Enums.GameState.Playing && health <= 0)
        {
            ChangeGameState(Enums.GameState.Lose);
        }
    }

    private void PrepareNextLevel()
    {
        scoreSO.ResetInt();
        healthSO.ResetInt();
    }

    public void ChangeGameState(Enums.GameState newGameState)
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
        scoreSO.IntChangeEvent.AddListener(CheckScore);
        healthSO.IntChangeEvent.AddListener(CheckHealth);
    }

    private void OnDisable()
    {
        gameStateAction -= ManageGame;
        scoreSO.IntChangeEvent.RemoveListener(CheckScore);
        healthSO.IntChangeEvent.RemoveListener(CheckHealth);
    }
}
