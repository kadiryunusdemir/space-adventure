using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Object = UnityEngine.Object;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera camera;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private IntSO meteorSO;
    
    private Action<Enums.GameState> gameStateAction;
    private Enums.GameState gameState;
    private LevelData levelData;
    private int totalMeteorCount;

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
                totalMeteorCount = GetTotalMeteorCount();
                ChangeGameState(Enums.GameState.Playing);
                break;
            case Enums.GameState.Playing:
                Time.timeScale = 1f;
                await enemySpawner.CreateAsteroidShower2(levelData);
                SoundManager.Instance.StartGameSound();
                break;
            case Enums.GameState.Win:
                Time.timeScale = 0;
                // TODO: handle this
                if (currentLevelIndex >= Enums.LevelIndex.Level3)
                {
                    ChangeGameState(Enums.GameState.GameEnded);
                    break;
                }
                SoundManager.Instance.PlaySound(Enums.Sound.Win);
                SoundManager.Instance.StopGameSound();
                await uiManager.DisplayRelatedPanel(Enums.GameState.Win, currentLevelIndex);
                levelManager.IncreaseLevelIndex();
                break;
            case Enums.GameState.Lose:
                Time.timeScale = 0;
                SoundManager.Instance.PlaySound(Enums.Sound.Lose);
                SoundManager.Instance.StopGameSound();
                await uiManager.DisplayRelatedPanel(Enums.GameState.Lose, currentLevelIndex);
                break;
            case Enums.GameState.Paused:
                Time.timeScale = 0;
                SoundManager.Instance.StopGameSound();
                await uiManager.DisplayRelatedPanel(Enums.GameState.Paused, currentLevelIndex);
                break;
            case Enums.GameState.GameEnded:
                // TODO: game end panel  
                Time.timeScale = 0;
                SoundManager.Instance.StopGameSound();
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
        camera.transform.DOShakePosition(0.4f, Vector3.one / 10);
        camera.transform.DOShakeRotation(0.4f, Vector3.one / 10);
            
        SoundManager.Instance.PlaySound(Enums.Sound.PlayerHit, transform.position);

        if (gameState == Enums.GameState.Playing && health <= 0)
        {
            SoundManager.Instance.PlaySound(Enums.Sound.PlayerDie, transform.position);

            ChangeGameState(Enums.GameState.Lose);
        }
    }

    private void PrepareNextLevel()
    {
        ObjectPoolManager.Instance.ResetAll();
        scoreSO.ResetInt();
        healthSO.ResetInt();
    }
    
    private int GetTotalMeteorCount()
    {
        int total = 0;
        foreach (var item in levelData.Waves)
        {
            total += item.enemyCount;
        }

        return total;
    }

    private void ManageMeteorDestroy(int count)
    {
        if (count == totalMeteorCount)
        {
            ChangeGameState(Enums.GameState.Win);
        }
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
        meteorSO.IntChangeEvent.AddListener(ManageMeteorDestroy);
        // scoreSO.IntChangeEvent.AddListener(CheckScore);
        healthSO.IntChangeEvent.AddListener(CheckHealth);
    }

    private void OnDisable()
    {
        gameStateAction -= ManageGame;
        meteorSO.IntChangeEvent.RemoveListener(ManageMeteorDestroy);
        // scoreSO.IntChangeEvent.RemoveListener(CheckScore);
        healthSO.IntChangeEvent.RemoveListener(CheckHealth);
    }
}
