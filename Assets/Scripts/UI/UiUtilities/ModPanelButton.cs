using System.IO;
using Dropbox;
using Dropbox.Utilities;
using TMPro;
using UI.Handlers;
using UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UiUtilities
{
    public class ModPanelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionTMP;
        [SerializeField] private TMP_Text titleTMP;
        [SerializeField] private RawImage preview;
        [SerializeField] private Button downloadButton;
        private DownloadHandler downloadHandler;
        private DetailsPage detailsPage;
        private Button modButton;
        private string title;
        private string description;
        private string category;
        private string previewPath;
        private string modPath;

        public string Category => category;

        public string Title => title;

        private void Start()
        {
            modButton = gameObject.GetComponent<Button>();
            downloadButton.onClick.AddListener(() => StartCoroutine(downloadHandler.Download(modPath)));
            modButton.onClick.AddListener(ActivateDetailsPage);
        }

        public void Init(Mod mod, DetailsPage details, DownloadHandler downloadHandler)
        {
            this.downloadHandler = downloadHandler;
            detailsPage = details;
            title = mod.title;
            description = mod.description;
            category = mod.category;
            previewPath = mod.preview_path;
            modPath = mod.file_path;
            titleTMP.text = Title;
            descriptionTMP.text = description;
            preview.texture = LoadPNG(Application.persistentDataPath + "/" + previewPath);
        }
        private Texture2D LoadPNG(string filePath)
        {

            Texture2D texture = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
            }
            return texture;
        }

        private void ActivateDetailsPage()
        {
            detailsPage.Show(title, description, previewPath,modPath, preview.texture);
        }
    }
}
