using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //дальше короче всякие приколы
    }
}
