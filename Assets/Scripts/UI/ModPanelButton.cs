using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ModPanelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionTMP;
    [SerializeField] private TMP_Text titleTMP;
    [SerializeField] private RawImage preview;
    private Page detailsPage;
    private Button button;
    private string title;
    private string description;
    private string category;
    private string previewPath;

    public string Category => category;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ActivateDetailsPage);
        
    }

    public void Init(Mod mod, Page details)
    {
        detailsPage = details;
        title = mod.title;
        description = mod.description;
        category = mod.category;
        previewPath = mod.preview_path;
        titleTMP.text = title;
        descriptionTMP.text = description;
        preview.texture = LoadPNG(previewPath);
    }
    private Texture2D LoadPNG(string filePath)
    {

        Texture2D texture = null;
        byte[] fileData;

        if (System.IO.File.Exists(filePath))
        {
            fileData = System.IO.File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
        }
        return texture;
    }

    private void ActivateDetailsPage()
    {
        detailsPage.Show();
    }
}
