using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CategoryButton : MonoBehaviour
{
    private Button button;
    private string categoryName;
    public Action<Button> OnCategorySelected;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CategorySelected);
    }

    private void CategorySelected()
    {
        OnCategorySelected(button);
        //TODO дальше короче всякие приколы
    }
}
