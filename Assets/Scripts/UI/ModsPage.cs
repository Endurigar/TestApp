using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ModsPage : MenuPages
{
    [SerializeField] private Page detailsPage;
    [SerializeField] private GameObject modsPanel;
    [Inject] private DropboxHandler dropboxHandler;

    private void Start()
    {
        dropboxHandler.OnModsPreviewDownloaded += ModsPanelSpawner;
    }

    private void ModsPanelSpawner()
    {
        for (int i = 0; i < dropboxHandler.ModsList.mods.Count; i++)
        {
            var newModsPanel = Instantiate(modsPanel, gameObject.transform).GetComponent<ModPanelButton>();
            newModsPanel.Init(dropboxHandler.ModsList.mods[i], detailsPage);
        }
    }
}
