using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ModsPage : MenuPages
{
    [SerializeField] private DetailsPage detailsPage;
    [SerializeField] private GameObject modsPanel;
    [SerializeField] private Transform layout;
    [SerializeField] private InputSearchHandler inputSearchHandler;
    [SerializeField] private CategoriesButtonsHandler categoriesButtonsHandler;
    [SerializeField] private Image dontFindImage;
    [Inject] private DropboxHandler dropboxHandler;
    [Inject] private DownloadHandler downloadHandler;
    private List<ModPanelButton> modPanelButtons;
    private string category= string.Empty;
    private string searchValue = string.Empty;

    protected override void Start()
    {
        base.Start();
        modPanelButtons = new List<ModPanelButton>();
        categoriesButtonsHandler.OnCategorySelected += GetCategory;
        categoriesButtonsHandler.OnCategoryUnselected += GetCategory;
        inputSearchHandler.OnValueChanged += GetSearchValue;
        dropboxHandler.OnModsPreviewDownloaded += ModsPanelSpawner;
    }

    private void ModsPanelSpawner()
    {
        for (int i = 0; i < dropboxHandler.ModsList.mods.Count; i++)
        {
            var newModsPanel = Instantiate(modsPanel, layout).GetComponent<ModPanelButton>();
            newModsPanel.Init(dropboxHandler.ModsList.mods[i], detailsPage, downloadHandler);
            modPanelButtons.Add(newModsPanel);
        }
    }

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
        dontFindImage.gameObject.SetActive(!results.Any());
        foreach (var mod in modPanelButtons)
        {
            mod.gameObject.SetActive(results.Contains(mod));
        }
    }

    private void GetCategory(string category)
    {
        this.category = category;
        Sort();
    }

    private void GetSearchValue(string value)
    {
        searchValue = value;
        Sort();
    }
}
