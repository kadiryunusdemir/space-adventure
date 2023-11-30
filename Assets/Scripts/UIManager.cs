using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private TextMeshProUGUI scoreTextValue;
    [SerializeField] private TextMeshProUGUI healthTextValue;
    
    private void Start()
    {
        scoreTextValue.text = scoreSO.Number.ToString();
        healthTextValue.text = healthSO.Number.ToString();
    }

    public void TestScore()
    {
        scoreSO.IncreaseInt(1);
    }
    
    public void TestHealth()
    {
        healthSO.DecreaseInt(1);
    }

    private void UpdateScoreUI(int score)
    {
        scoreTextValue.text = scoreSO.Number.ToString();
    }
    
    private void UpdateHealthUI(int health)
    {
        healthTextValue.text = healthSO.Number.ToString();
    }

    private void OnEnable()
    {
        scoreSO.IntChangeEvent.AddListener(UpdateScoreUI);
        healthSO.IntChangeEvent.AddListener(UpdateHealthUI);
    }

    private void OnDisable()
    {
        scoreSO.IntChangeEvent.RemoveListener(UpdateScoreUI);
        healthSO.IntChangeEvent.RemoveListener(UpdateHealthUI);
    }
}
