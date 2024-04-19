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
    public class DropboxHandler : MonoBehaviour
    {
        [Inject] private LoadingBar loadingBar;
        [Inject] private DownloadFailHandler downloadFailHandler;
        public Action OnConfigDownloaded;
        public Action OnModsPreviewDownloaded;
        private readonly string Path = "";
        public ModsList ModsList { get; private set; }

        private IEnumerator Start()
        {
            Debug.Log(Path);
            yield return StartCoroutine(Initialize());
            yield return StartCoroutine(DownloadConfigs());
        }

        private IEnumerator Initialize()
        {
            var taskInitialize = DropboxHelper.Initialize();
            yield return new WaitUntil(() => taskInitialize.IsCompleted);
        }

        private IEnumerator DownloadConfigs()
        {
            loadingBar.ResetValue();
            loadingBar.Show();
            var taskGetFiles = DropboxHelper.GetFiles(Path);
            yield return new WaitUntil(() => taskGetFiles.IsCompleted);
            if (!taskGetFiles.IsCompleted) yield break;

            var jsonObject = JsonConvert.DeserializeObject<EntriesData>(taskGetFiles.Result);
            var configsList = jsonObject.Entries.Where(element => System.IO.Path.GetExtension(element.name) == ".json")
                .ToList();
            for (var i = 0; i < configsList.Count; i++)
            {
                var file = configsList[i];
                if (file.tag == "file" && System.IO.Path.GetExtension(file.name) == ".json")
                {
                    yield return StartCoroutine(DownloadByName(file.path_lower));
                    loadingBar.SetBarValue(i / ((float)configsList.Count - 2));
                }
            }

            var modsJson = File.ReadAllText(Application.persistentDataPath + "/" + "mods.json");
            ModsList = JsonConvert.DeserializeObject<ModsList>(modsJson);
            OnConfigDownloaded?.Invoke();
            StartCoroutine(DowloadPreview());
        }

        public IEnumerator DownloadByName(string path)
        {
            var taskDownload = DropboxHelper.DownloadAndSaveFile(path);
            yield return new WaitUntil(() => taskDownload.IsCompleted);
            if (taskDownload.IsFaulted) downloadFailHandler.Show();
        }

        private IEnumerator DowloadPreview()
        {
            foreach (var mod in ModsList.mods)
            {
                if (File.Exists(Application.persistentDataPath + "/" + mod.preview_path)) continue;
                var taskDownload = DropboxHelper.DownloadAndSaveFile(mod.preview_path);
                yield return new WaitUntil(() => taskDownload.IsCompleted);
            }

            OnModsPreviewDownloaded?.Invoke();
            loadingBar.Hide();
        }

        public IEnumerator DownloadMod(string path)
        {
            float currentValue = 0;
            loadingBar.ResetValue();
            loadingBar.Show();
            DOTween.To(() => currentValue, x => currentValue = x, 1, 1)
                .OnUpdate(() => { loadingBar.SetBarValue(currentValue); });
            yield return DownloadByName(path);
            loadingBar.Hide();
        }
    }
}