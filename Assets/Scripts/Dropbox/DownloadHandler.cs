using System.Collections;
using System.IO;
using UnityEngine;
using Zenject;

namespace Dropbox
{
    public class DownloadHandler : MonoBehaviour
    {
        [Inject] private DropboxHandler dropboxHandler;

        public IEnumerator Download(string relativePath)
        {
            yield return StartCoroutine(dropboxHandler.DownloadMod(relativePath));
            string filePath = Application.persistentDataPath + "/" + relativePath;
            Debug.Log(filePath);
            new NativeShare().AddFile(filePath)
                .SetSubject("DownloadedMod")
                .SetCallback((result, shareTarget) =>
                    Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
        }
    }
}
