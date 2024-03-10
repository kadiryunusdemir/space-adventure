using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

public class SurveyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subTitle;
    [SerializeField] private Button normalEmotionButton;
    [SerializeField] private Button happyEmotionButton;
    [SerializeField] private Button angryEmotionButton;
    [SerializeField] private Button surprisedEmotionButton;

    public bool isButtonClicked { get; private set; } = false;
    public Enums.Emotion selectedEmotionEnum { get; private set; } = Enums.Emotion.Default;
    
    private void Awake()
    {
        normalEmotionButton.onClick.AddListener(async () => 
        {
            SelectEmotion(Enums.Emotion.Normal);
            await DeactivatePanel();
        });
        happyEmotionButton.onClick.AddListener(async () => 
        {
            SelectEmotion(Enums.Emotion.Happy);
            await DeactivatePanel();
        });
        angryEmotionButton.onClick.AddListener(async () => 
        {
            SelectEmotion(Enums.Emotion.Angry);
            await DeactivatePanel();
        });
        surprisedEmotionButton.onClick.AddListener(async () => 
        {
            SelectEmotion(Enums.Emotion.Suprised);
            await DeactivatePanel();
        });
    }
    
    public async UniTask DisplayPanel(string titleText, string subTitleText)
    {
        isButtonClicked = false;
        title.text = titleText;
        subTitle.text = subTitleText;
        // image.sprite = sprite; 
        gameObject.SetActive(true); 
        await transform.DOScale(1, 1f).From(0).SetEase(Ease.OutBack).SetUpdate(true); //ignore Unity's Time.timeScale
    }

    private async UniTask DeactivatePanel()
    {
        await transform.DOScale(0, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
        gameObject.SetActive(false);
        isButtonClicked = true;
    }
    
    private void SelectEmotion(Enums.Emotion emotion)
    {
        selectedEmotionEnum = emotion;
    }
}
