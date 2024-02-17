using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private TextMeshProUGUI scoreTextValue;
    [SerializeField] private TextMeshProUGUI healthTextValue;
    [SerializeField] private GameObject mask;
    [SerializeField] private UIPanel uiPanel;
    // TODO: surveyPanel

    private void Awake()
    {
        mask.gameObject.SetActive(false);
        uiPanel.gameObject.SetActive(false);
    }

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

    public async UniTask DisplayRelatedPanel(Enums.GameState currentGameState, Enums.LevelIndex levelIndex)
    {
        var levelInt = (int)levelIndex;
        
        mask.gameObject.SetActive(true);
        
        switch (currentGameState)
        {
            case Enums.GameState.Default:
                break;
            case Enums.GameState.MainMenu:
                break;
            case Enums.GameState.Starting:
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.Win:
                await uiPanel.DisplayPanel($"You Win Level - {levelInt}" ,
                    "TODO: survey",
                    () => PanelAction(Enums.GameState.Starting),
                    () => PanelAction(Enums.GameState.MainMenu));
                break;
            case Enums.GameState.Lose:
                await uiPanel.DisplayPanel($"You Lose Level - {levelInt}",
                    "TODO: survey",
                    () => PanelAction(Enums.GameState.Starting),
                    () => PanelAction(Enums.GameState.MainMenu));
                break;
            case Enums.GameState.Paused:
                await uiPanel.DisplayPanel("Do you want to play more?",
                    "TODO: survey",
                    () => PanelAction(Enums.GameState.Playing),
                    () => PanelAction(Enums.GameState.MainMenu));
                break;
            case Enums.GameState.GameEnded:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentGameState), currentGameState, null);
        }
    }

    private void PanelAction(Enums.GameState nextGameState)
    {
        mask.SetActive(false);
        GameManager.Instance.ChangeGameState(nextGameState);
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
