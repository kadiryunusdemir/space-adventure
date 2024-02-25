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
        var currentLevelIndex = levelManager.GetCurrentLevelIndex();
        
        // Debug.Log("gamestate: "+ currentGameState);
        switch (currentGameState)
        {
            case Enums.GameState.Default:
                break;
            case Enums.GameState.MainMenu:
                levelManager.OpenMainMenu();
                break;
            case Enums.GameState.Starting:
                PrepareNextLevel();
                levelData = levelManager.GetLevelData(currentLevelIndex);
                await enemySpawner.CreateAsteroidShower(levelData);
                ChangeGameState(Enums.GameState.Playing);
                break;
            case Enums.GameState.Playing:
                Time.timeScale = 1f;
                break;
            case Enums.GameState.Win:
                Time.timeScale = 0;
                // TODO: handle this
                if (currentLevelIndex >= Enums.LevelIndex.Level3)
                {
                    ChangeGameState(Enums.GameState.GameEnded);
                    break;
                }
                await uiManager.DisplayRelatedPanel(Enums.GameState.Win, currentLevelIndex);
                levelManager.IncreaseLevelIndex();
                break;
            case Enums.GameState.Lose:
                Time.timeScale = 0;
                await uiManager.DisplayRelatedPanel(Enums.GameState.Lose, currentLevelIndex);
                break;
            case Enums.GameState.Paused:
                Time.timeScale = 0;
                await uiManager.DisplayRelatedPanel(Enums.GameState.Paused, currentLevelIndex);
                break;
            case Enums.GameState.GameEnded:
                // TODO: game end panel  
                Time.timeScale = 0;
                Debug.Log("Last level is played");
                ChangeGameState(Enums.GameState.MainMenu);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void CheckScore(int score)
    {
        if (gameState == Enums.GameState.Playing && score > 5)
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
        ObjectPoolManager.Instance.ResetAll();
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

    public void PauseGame()
    {
        ChangeGameState(Enums.GameState.Paused);
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
