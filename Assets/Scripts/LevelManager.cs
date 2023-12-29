using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using static Utilities.Enums;

public class LevelManager : MonoBehaviour
{
    public LevelData GetLevelData(Enums.LevelIndex levelIndex)
    {
        var levelData = Resources.Load<LevelData>($"Levels/{levelIndex}");
        return levelData;
    }
    public LevelIndex GetCurrentLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        LevelIndex level = (LevelIndex)Enum.Parse(typeof(LevelIndex), levelName, true);
        return level;
    }
    public void OpenLevelWithId(int levelId)
    {
        string currentLevel = "Level" + levelId;
        SceneManager.LoadScene(currentLevel);
    }
    public void OpenLevelWithLevelIndex(LevelIndex level)
    {
        string Level = level.ToString();
        SceneManager.LoadScene(Level);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
