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
    private List<Button> categoryButtons;
    private Button latestSelectedButton;
    public Action<string> OnCategorySelected;
    public Action<string> OnCategoryUnselected;

    private void Start()
    {
        dropboxHandler.OnConfigDownloaded += OnConfigDownloaded;
        categoryButtons = new List<Button>();
    }

    private void OnConfigDownloaded()
    {
        categories = dropboxHandler.ModsList.categories.ToArray();
        ButtonSpawner();
    }

    private void ButtonSpawner()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            var newCategoryButton = Instantiate(categoryButton, categoriesGrid);
            newCategoryButton.transform.GetComponentInChildren<TMP_Text>().text = categories[i];
            var categoryName = categories[i];
            newCategoryButton.onClick.AddListener((() => ChooseCategoryButton(newCategoryButton, categoryName)));
            categoryButtons.Add(newCategoryButton);
        }
    }

    private void ChooseCategoryButton(Button SelectedButton, string categoryName)
    {
        if (latestSelectedButton == SelectedButton)
        {
            latestSelectedButton.GetComponent<Image>().sprite = categoryUnactive;
            OnCategoryUnselected(string.Empty);
            return;
        }
        if (latestSelectedButton != null)
        {
            latestSelectedButton.GetComponent<Image>().sprite = categoryUnactive;
            OnCategoryUnselected(string.Empty);
        }
        latestSelectedButton = SelectedButton;
        SelectedButton.GetComponent<Image>().sprite = categoryActive;
        OnCategorySelected(categoryName);
    }
}
