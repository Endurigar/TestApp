using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPage : Page
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(PageHide);
    }
    
    private void PageHide()
    {
        Hide();
    }
}
