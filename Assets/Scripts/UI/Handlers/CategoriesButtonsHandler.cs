using System;
using System.Collections.Generic;
using Dropbox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Handlers
{
    // Class responsible for handling category buttons in UI
    public class CategoriesButtonsHandler : MonoBehaviour
    {
        // Serialized fields for inspector accessibility
        [SerializeField] private Button categoryButton;
        [SerializeField] private Transform categoriesGrid;
        [SerializeField] private Sprite categoryActive;
        [SerializeField] private Sprite categoryInactive;
        [Inject] private DropboxHandler dropboxHandler;

        // Array to hold categories
        private string[] categories;
        // List to hold instantiated category buttons
        private List<Button> categoryButtons;
        // Reference to the last selected category button
        private Button latestSelectedButton;

        // Events to be invoked when category is selected or unselected
        public Action<string> OnCategorySelected;
        public Action<string> OnCategoryUnselected;

        // Start is called before the first frame update
        private void Start()
        {
            // Subscribe to event triggered when configuration is downloaded
            dropboxHandler.OnConfigDownloaded += OnConfigDownloaded;
            // Initialize list to hold category buttons
            categoryButtons = new List<Button>();
        }

        // Method called when configuration is downloaded
        private void OnConfigDownloaded()
        {
            // Get categories from ModsList
            categories = dropboxHandler.ModsList.categories.ToArray();
            // Instantiate category buttons
            ButtonSpawner();
        }

        // Method to instantiate category buttons
        private void ButtonSpawner()
        {
            // Loop through categories
            for (var i = 0; i < categories.Length; i++)
            {
                // Instantiate new category button
                var newCategoryButton = Instantiate(categoryButton, categoriesGrid);
                // Set category text on button
                newCategoryButton.transform.GetComponentInChildren<TMP_Text>().text = categories[i];
                var categoryName = categories[i];
                // Add listener to handle button click
                newCategoryButton.onClick.AddListener(() => ChooseCategoryButton(newCategoryButton, categoryName));
                // Add button to list
                categoryButtons.Add(newCategoryButton);
            }
        }

        // Method to handle category button selection
        private void ChooseCategoryButton(Button selectedButton, string categoryName)
        {
            // If the selected button is the same as the last selected button
            if (latestSelectedButton == selectedButton)
            {
                // Deselect the button
                latestSelectedButton.GetComponent<Image>().sprite = categoryInactive;
                // Invoke event for category unselected
                OnCategoryUnselected(string.Empty);
                return;
            }

            // If there was a previously selected button
            if (latestSelectedButton != null)
            {
                // Deselect the previous button
                latestSelectedButton.GetComponent<Image>().sprite = categoryInactive;
                // Invoke event for category unselected
                OnCategoryUnselected(string.Empty);
            }

            // Set the latest selected button to the current selected button
            latestSelectedButton = selectedButton;
            // Set the selected button as active
            selectedButton.GetComponent<Image>().sprite = categoryActive;
            // Invoke event for category selected
            OnCategorySelected(categoryName);
        }
    }
}

