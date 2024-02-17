using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using static Utilities.Enums;

public class LevelManager : MonoBehaviour
{
    private LevelIndex currentLevelIndex = LevelIndex.Default;
    
    public LevelData GetLevelData(Enums.LevelIndex levelIndex)
    {
        var levelData = Resources.Load<LevelData>($"Levels/{levelIndex}");
        return levelData;
    }
    public LevelIndex GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public LevelIndex IncreaseLevelIndex()
    {
        currentLevelIndex++;
        return currentLevelIndex;
    }

    public void ChooseLevelFromMenu(LevelIndex level)
    {
        currentLevelIndex = level;
        GameManager.Instance.ChangeGameState(GameState.Starting);
    } 
    
    // TODO: bu fonksiyon, üstteki ile değiştirilecek
    public void OpenLevelWithId(int levelId)
    {
        string currentLevel = "Level" + levelId;
        SceneManager.LoadScene(currentLevel);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
