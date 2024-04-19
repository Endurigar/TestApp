using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingBar : Page
{
    private const float Duration = 0.5f;
    [SerializeField] private Image bar;
    
    public void SetBarValue(float value)
    {
        Debug.Log(value);
        bar.DOFillAmount(value, Duration);
    }

    public void ResetValue()
    {
        bar.fillAmount = 0;
    }
}
