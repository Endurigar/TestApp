using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TintText : MonoBehaviour,  IDeselectHandler, ISelectHandler
{
    private Button button;
    private TMP_Text text;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        text = gameObject.GetComponentInChildren<TMP_Text>();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        text.color = button.colors.selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = button.colors.normalColor;
    }
}
