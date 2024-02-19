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

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subTitle;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button actionButton;

    public async UniTask DisplayPanel(string titleText, string subTitleText, UnityAction yesAction, UnityAction noAction)
    {
        title.text = titleText;
        subTitle.text = subTitleText;
        cancelButton.onClick.AddListener(async () => 
        {
            await DeactivatePanel();
            noAction();
        });
        actionButton.onClick.AddListener(async () => 
        {
            await DeactivatePanel();
            yesAction();
        });
        // image.sprite = sprite; 
        gameObject.SetActive(true); 
        await transform.DOScale(1, 1f).From(0).SetEase(Ease.OutBack).SetUpdate(true); //ignore Unity's Time.timeScale
    }

    private async UniTask DeactivatePanel()
    {
        await transform.DOScale(0, 0.2f).SetEase(Ease.Linear).SetUpdate(true);
        gameObject.SetActive(false);
    }
}
