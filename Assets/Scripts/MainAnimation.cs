using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MainAnimation : MonoBehaviour
{
    [SerializeField] private GameObject keyboard;
    [SerializeField] private GameObject playButton;
    
    private void Start()
    {
        keyboard.transform.DOScale(1f, 1.5f).From(0).SetEase(Ease.OutBack).OnComplete(() =>
        {
            keyboard.transform.DORotate(new Vector3(30, -20, 0), 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
        
        playButton.transform.DOScale(1f, 1f).From(0).SetEase(Ease.OutBack).OnComplete(() =>
        {
            playButton.transform.DOScale(1.5f, 1f).From(1).SetLoops(-1, LoopType.Yoyo);
        });
    }
}
