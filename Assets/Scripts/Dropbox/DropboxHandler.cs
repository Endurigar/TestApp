using System;
using System.Collections;
using System.IO;
using System.Linq;
using DG.Tweening;
using Dropbox.Data;
using Dropbox.Utilities;
using Newtonsoft.Json;
using Plugins.Dropbox;
using UI.Pages;
using UnityEngine;
using Zenject;

namespace Dropbox
{
    // Class responsible for handling Dropbox operations
    public class DropboxHandler : MonoBehaviour
    {
        // Injected dependencies
        [Inject] private LoadingBar loadingBar;
        [Inject] private DownloadFailHandler downloadFailHandler;

        // Events to be invoked when certain actions are completed
        public Action OnConfigDownloaded;
        public Action OnModsPreviewDownloaded;

        // Path to Dropbox directory
        private readonly string Path = "";

        // Property to access the ModsList
        public ModsList ModsList { get; private set; }

        // Coroutine called on start
        private IEnumerator Start()
        {
            // Output path to console
            Debug.Log(Path);
            // Initialize Dropbox
            yield return StartCoroutine(Initialize());
            // Download configuration files
            yield return StartCoroutine(DownloadConfigs());
        }

        // Coroutine to initialize Dropbox
        private IEnumerator Initialize()
        {
            var taskInitialize = DropboxHelper.Initialize();
            yield return new WaitUntil(() => taskInitialize.IsCompleted);
        }

        // Coroutine to download configuration files
        private IEnumerator DownloadConfigs()
        {
            // Reset and show loading bar
            loadingBar.ResetValue();
            loadingBar.Show();
            // Get files from Dropbox
            var taskGetFiles = DropboxHelper.GetFiles(Path);
            yield return new WaitUntil(() => taskGetFiles.IsCompleted);
            // Check if file retrieval is completed
            if (!taskGetFiles.IsCompleted) yield break;

            // Deserialize JSON response
            var jsonObject = JsonConvert.DeserializeObject<EntriesData>(taskGetFiles.Result);
            // Filter JSON entries to get configuration files
            var configsList = jsonObject.Entries.Where(element => System.IO.Path.GetExtension(element.name) == ".json")
                .ToList();
            // Loop through configuration files
            for (var i = 0; i < configsList.Count; i++)
            {
                var file = configsList[i];
                if (file.tag == "file" && System.IO.Path.GetExtension(file.name) == ".json")
                {
                    // Download configuration file by name
                    yield return StartCoroutine(DownloadByName(file.path_lower));
                    // Update loading bar progress
                    loadingBar.SetBarValue(i / ((float)configsList.Count - 2));
                }
            }

            // Read mods.json file
            var modsJson = File.ReadAllText(Application.persistentDataPath + "/" + "mods.json");
            // Deserialize mods.json into ModsList object
            ModsList = JsonConvert.DeserializeObject<ModsList>(modsJson);
            // Invoke event indicating config files are downloaded
            OnConfigDownloaded?.Invoke();
            // Start downloading mod previews
            StartCoroutine(DowloadPreview());
        }

        // Coroutine to download file by name
        public IEnumerator DownloadByName(string path)
        {
            var taskDownload = DropboxHelper.DownloadAndSaveFile(path);
            yield return new WaitUntil(() => taskDownload.IsCompleted);
            // Show download fail handler if download is faulted
            if (taskDownload.IsFaulted) downloadFailHandler.Show();
        }

        // Coroutine to download mod previews
        private IEnumerator DowloadPreview()
        {
            foreach (var mod in ModsList.mods)
            {
                // Check if mod preview exists, if not download it
                if (File.Exists(Application.persistentDataPath + "/" + mod.preview_path)) continue;
                var taskDownload = DropboxHelper.DownloadAndSaveFile(mod.preview_path);
                yield return new WaitUntil(() => taskDownload.IsCompleted);
            }

            // Invoke event indicating mod previews are downloaded
            OnModsPreviewDownloaded?.Invoke();
            // Hide loading bar
            loadingBar.Hide();
        }

        // Coroutine to download a specific mod
        public IEnumerator DownloadMod(string path)
        {
            // Initialize progress value
            float currentValue = 0;
            // Reset and show loading bar
            loadingBar.ResetValue();
            loadingBar.Show();
            // Animate loading bar progress
            DOTween.To(() => currentValue, x => currentValue = x, 1, 1)
                .OnUpdate(() => { loadingBar.SetBarValue(currentValue); });
            // Download mod by name
            yield return DownloadByName(path);
            // Hide loading bar
            loadingBar.Hide();
        }
    }
}
