using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DownloadHandler : MonoBehaviour
{
    [Inject] private DropboxHandler dropboxHandler;
    
    public IEnumerator Download(string relativePath)
    {
        yield return StartCoroutine(dropboxHandler.DownloadMod(relativePath));
        string filePath = Path.Combine( Application.persistentDataPath, relativePath );
        new NativeShare().AddFile( filePath )
            .SetSubject("DownloadedMod")
            .SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
            .Share();
    }
}
