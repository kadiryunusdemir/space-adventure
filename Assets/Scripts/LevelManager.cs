using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    public LevelData GetLevelData(Enums.LevelIndex levelIndex)
    {
        var levelData = Resources.Load<LevelData>($"Levels/{levelIndex}");
        return levelData;
    }
}
