using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Newtonsoft.Json;
using Plugins.Dropbox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class DropboxHandler : MonoBehaviour
{
    [Inject] private LoadingBar loadingBar;
    [Inject] private DownloadFailHandler downloadFailHandler;
    public Action OnConfigDownloaded;
    public Action OnModsPreviewDownloaded;
    private ModsList modsList;
    private string Path = "";

    public ModsList ModsList => modsList;

    private IEnumerator Start()
    {
        Debug.Log(Path);
        yield return StartCoroutine(Initialize());
        yield return StartCoroutine(DownloadConfigs());
    }

    IEnumerator Initialize()
    {
        var taskInitialize = DropboxHelper.Initialize();
        yield return new WaitUntil(() => taskInitialize.IsCompleted);
    }
    IEnumerator DownloadConfigs()
    {
        loadingBar.ResetValue();
        loadingBar.Show();
        var taskGetFiles = DropboxHelper.GetFiles(Path);
        yield return new WaitUntil(() => taskGetFiles.IsCompleted);
        if (!taskGetFiles.IsCompleted)
        {
            yield break;
        }
        var jsonObject = JsonConvert.DeserializeObject<EntriesData>(taskGetFiles.Result);
        var configsList = jsonObject.Entries.Where((element) => System.IO.Path.GetExtension(element.name) == ".json").ToList();
        for (int i = 0; i < configsList.Count; i++)
        {
            var file = configsList[i];
            if (file.tag == "file" && System.IO.Path.GetExtension(file.name) == ".json")
            {
                yield return StartCoroutine(DownloadByName(file.path_lower));
                //Debug.Log(((float)i/(float)configsList.Count));
                loadingBar.SetBarValue((float)i/((float)configsList.Count-2));
            }
        }
        var modsJson = File.ReadAllText(Application.persistentDataPath + "/" + "mods.json");
        modsList = JsonConvert.DeserializeObject<ModsList>(modsJson);
        OnConfigDownloaded?.Invoke();
        StartCoroutine(DowloadPreview());
    }

    public IEnumerator DownloadByName(string path)
    {
        var taskDownload = DropboxHelper.DownloadAndSaveFile(path);
        yield return new WaitUntil(() => taskDownload.IsCompleted);
        if (taskDownload.IsFaulted)
        {
            downloadFailHandler.Show();
        }
    }

    IEnumerator DowloadPreview()
    {
        foreach (var mod in modsList.mods)
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
            .OnUpdate(() => {loadingBar.SetBarValue(currentValue);
            });
        yield return DownloadByName(path);
        loadingBar.Hide();
    }
}
