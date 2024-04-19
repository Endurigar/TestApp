using System.Collections.Generic;
using System.Linq;
using Dropbox;
using UI.Handlers;
using UI.UiUtilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Pages
{
    // Class representing the Mods page in UI
    public class ModsPage : MenuPages
    {
        // Serialized fields for inspector accessibility
        [SerializeField] private DetailsPage detailsPage;                      // Reference to details page
        [SerializeField] private GameObject modsPanel;                         // Prefab for mod panel
        [SerializeField] private Transform layout;                            // Layout to hold mod panels
        [SerializeField] private InputSearchHandler inputSearchHandler;       // Reference to search handler
        [SerializeField] private CategoriesButtonsHandler categoriesButtonsHandler; // Reference to categories buttons handler
        [SerializeField] private Image dontFindImage;                         // Image to display when no results found
        [Inject] private DropboxHandler dropboxHandler;                       // Reference to DropboxHandler for data access
        [Inject] private DownloadHandler downloadHandler;                     // Reference to download handler

        // List to hold instantiated mod panel buttons
        private List<ModPanelButton> modPanelButtons;
        // Category selected for filtering mods
        private string category = string.Empty;
        // Search value entered for filtering mods
        private string searchValue = string.Empty;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // Initialize list to hold mod panel buttons
            modPanelButtons = new List<ModPanelButton>();
            // Subscribe to category selection and deselection events
            categoriesButtonsHandler.OnCategorySelected += GetCategory;
            categoriesButtonsHandler.OnCategoryUnselected += GetCategory;
            // Subscribe to search value change event
            inputSearchHandler.OnValueChanged += GetSearchValue;
            // Trigger method to spawn mod panels when mod previews are downloaded
            dropboxHandler.OnModsPreviewDownloaded += ModsPanelSpawner;
        }

        // Method to instantiate mod panels based on downloaded mod previews
        private void ModsPanelSpawner()
        {
            for (int i = 0; i < dropboxHandler.ModsList.mods.Count; i++)
            {
                // Instantiate new mod panel button
                var newModsPanel = Instantiate(modsPanel, layout).GetComponent<ModPanelButton>();
                // Initialize mod panel button with mod data
                newModsPanel.Init(dropboxHandler.ModsList.mods[i], detailsPage, downloadHandler);
                // Add mod panel button to the list
                modPanelButtons.Add(newModsPanel);
            }
        }

        // Method to filter mod panels based on category and search value
        private void Sort()
        {
            var results = modPanelButtons.Where(element =>
            {
                if (!string.IsNullOrEmpty(category))
                {
                    return category == element.Category && element.Title.Contains(searchValue);
                }
                else
                {
                    return element.Title.Contains(searchValue);
                }
            });
            // Show or hide "No Results" image based on whether there are any results
            dontFindImage.gameObject.SetActive(!results.Any());
            // Set mod panels active/inactive based on filter results
            foreach (var mod in modPanelButtons)
            {
                mod.gameObject.SetActive(results.Contains(mod));
            }
        }

        // Method to handle category selection or deselection
        private void GetCategory(string category)
        {
            this.category = category;
            // Apply filtering
            Sort();
        }

        // Method to handle search value change
        private void GetSearchValue(string value)
        {
            searchValue = value;
            // Apply filtering
            Sort();
        }
    }
}

