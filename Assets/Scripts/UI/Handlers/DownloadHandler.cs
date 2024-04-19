using System.Collections;
using System.IO;
using Dropbox;
using UnityEngine;
using Zenject;

namespace UI.Handlers
{
    public class DownloadHandler : MonoBehaviour
    {
        [Inject] private DropboxHandler dropboxHandler;

        public IEnumerator Download(string relativePath)
        {
            yield return StartCoroutine(dropboxHandler.DownloadMod(relativePath));
            var filePath = Path.Combine(Application.persistentDataPath, relativePath);
            new NativeShare().AddFile(filePath)
                .SetSubject("DownloadedMod")
                .SetCallback((result, shareTarget) =>
                    Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
        }
    }
}
