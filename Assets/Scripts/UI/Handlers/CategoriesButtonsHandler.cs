using System;
using System.Collections.Generic;
using Dropbox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Handlers
{
    public class CategoriesButtonsHandler : MonoBehaviour
    {
        [SerializeField] private Button categoryButton;
        [SerializeField] private Transform categoriesGrid;
        [SerializeField] private Sprite categoryActive;
        [SerializeField] private Sprite categoryInactive;
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
            for (var i = 0; i < categories.Length; i++)
            {
                var newCategoryButton = Instantiate(categoryButton, categoriesGrid);
                newCategoryButton.transform.GetComponentInChildren<TMP_Text>().text = categories[i];
                var categoryName = categories[i];
                newCategoryButton.onClick.AddListener(() => ChooseCategoryButton(newCategoryButton, categoryName));
                categoryButtons.Add(newCategoryButton);
            }
        }

        private void ChooseCategoryButton(Button selectedButton, string categoryName)
        {
            if (latestSelectedButton == selectedButton)
            {
                latestSelectedButton.GetComponent<Image>().sprite = categoryInactive;
                OnCategoryUnselected(string.Empty);
                return;
            }

            if (latestSelectedButton != null)
            {
                latestSelectedButton.GetComponent<Image>().sprite = categoryInactive;
                OnCategoryUnselected(string.Empty);
            }

            latestSelectedButton = selectedButton;
            selectedButton.GetComponent<Image>().sprite = categoryActive;
            OnCategorySelected(categoryName);
        }
    }
}


