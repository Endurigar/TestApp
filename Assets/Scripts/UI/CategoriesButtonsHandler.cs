using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button categoryButton;
    [SerializeField] private Transform categoriesGrid;
    [SerializeField] private Sprite categoryActive;
    [SerializeField] private Sprite categoryUnactive;
    private string[] categories = new []{"cat1","cat2","cat4","cat3"};
    private Button[] categoryButtons;
    private Button latestSelectedButton;

    private void Start()
    {
        categoryButtons = new Button[categories.Length];
        ButtonSpawner();
    }

    private void ButtonSpawner()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            var newCategoryButton = Instantiate(categoryButton, categoriesGrid);
            newCategoryButton.transform.GetComponentInChildren<TMP_Text>().text = categories[i];
            newCategoryButton.transform.GetComponent<CategoryButton>().OnCategorySelected += Test;
        }
    }

    private void Test(Button SelectedButton)
    {
        if (latestSelectedButton == SelectedButton)
        {
            Debug.Log("=)))");
            SelectedButton.GetComponent<Image>().sprite = categoryUnactive;
            return;
        }
        if (latestSelectedButton != null)
        {
            latestSelectedButton.GetComponent<Image>().sprite = categoryUnactive;
        }
        latestSelectedButton = SelectedButton;
        SelectedButton.GetComponent<Image>().sprite = categoryActive;
    }
}
