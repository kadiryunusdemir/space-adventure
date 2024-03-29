using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelData), menuName  = "Scriptable Objects/Level Data")]
public class LevelData : ScriptableObject
{
    public Data[] Waves;
}

[Serializable]
public struct Data
{
    public int WaveDensityPercentage;
    public int fireDelay;
    public int enemyCount;
    public int[] delayAmount;
    public int[] enemyType;
    public int[] enemySpawnPoint;
    // public int WaveEnemyCount;
    [NonSerialized] public int DestroyedEnemyCount;
}
