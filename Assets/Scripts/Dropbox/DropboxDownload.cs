using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.Dropbox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class DropboxDownload : MonoBehaviour
{
    private string Path = "";
    private IEnumerator Start()
    {
        Debug.Log(Path);
        yield return StartCoroutine(Initialize());
        yield return StartCoroutine(Download());
    }

    IEnumerator Initialize()
    {
        var taskInitialize = DropboxHelper.Initialize();
        yield return new WaitUntil(() => taskInitialize.IsCompleted);
    }
    IEnumerator Download()
    {
        var taskGetFiles = DropboxHelper.GetFiles(Path);
        yield return new WaitUntil(() => taskGetFiles.IsCompleted);
        if (!taskGetFiles.IsCompleted)
        {
            yield break;
        }
        var jsonObject = JsonConvert.DeserializeObject<EntriesData>(taskGetFiles.Result);
        foreach (var VARIABLE in jsonObject.Entries)
        {
            if (VARIABLE.tag == "file")
            {
                yield return StartCoroutine(DownloadByName(VARIABLE.path_lower));  
            }
        };
    }

    IEnumerator DownloadByName(string path)
    {
        var taskDownload = DropboxHelper.DownloadAndSaveFile(path);
        yield return new WaitUntil(() => taskDownload.IsCompleted);
    }
}
