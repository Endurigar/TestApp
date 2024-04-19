using Dropbox;
using TMPro;
using UI.Handlers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Pages
{
    public class DetailsPage : Page
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button downloadButton;
        [SerializeField] private TMP_Text descriptionTMP;
        [SerializeField] private TMP_Text titleTMP;
        [SerializeField] private RawImage preview;
        [Inject] private DownloadHandler downloadHandler;
        private string previewPath;
        private string modPath;

        protected override void Start()
        {
            base.Start();
            backButton.onClick.AddListener(PageHide);
            downloadButton.onClick.AddListener(() => StartCoroutine(downloadHandler.Download(modPath)));
        }

        public void Show(string title, string description, string previewPath,string modPath, Texture preview)
        {
            base.Show();
            this.modPath = modPath;
            descriptionTMP.text = description;
            titleTMP.text = title;
            this.preview.texture = preview;
            this.previewPath = previewPath;
        }

        private void PageHide()
        {
            Hide();
        }
    }
}
