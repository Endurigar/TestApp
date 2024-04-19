using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CategoriesButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button categoryButton;
    [SerializeField] private Transform categoriesGrid;
    [SerializeField] private Sprite categoryActive;
    [SerializeField] private Sprite categoryUnactive;
    [Inject] private DropboxHandler dropboxHandler;
    private string[] categories;
    private Button[] categoryButtons;
    private Button latestSelectedButton;

    private void Start()
    {
        dropboxHandler.OnConfigDownloaded += OnConfigDownloaded;
    }

    private void OnConfigDownloaded()
    {
        categories = dropboxHandler.ModsList.categories.ToArray();
        categoryButtons = new Button[categories.Length];
        ButtonSpawner();
    }

    private void ButtonSpawner()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            var newCategoryButton = Instantiate(categoryButton, categoriesGrid);
            newCategoryButton.transform.GetComponentInChildren<TMP_Text>().text = categories[i];
            newCategoryButton.transform.GetComponent<CategoryButton>().OnCategorySelected += ChooseCategoryButton;
        }
    }

    private void ChooseCategoryButton(Button SelectedButton)
    {
        if (latestSelectedButton == SelectedButton)
        {
            latestSelectedButton.GetComponent<Image>().sprite = categoryUnactive;
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
