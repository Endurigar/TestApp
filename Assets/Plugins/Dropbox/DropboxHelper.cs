using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.Dropbox
{
    public static class DropboxHelper
    {
        // paste from dropbox console
        private const string AppKey = "grr8w8pnylo5onj";
        private const string AppSecret = "jlv4xmimt72motr";

#if UNITY_EDITOR
        // paste auth code from your browser, or given from production department, here. Only valid for about 10 minutes.
        // You can remove it after getting refreshToken
        private const string AuthCode = "I0w2EF3uclgAAAAAAAAAFwVq1bBr2axW584eFHVdFvk";
#endif

        // paste from GetRefreshToken() result, value of "refresh_token"
        private const string RefreshToken = "xjs_rWL6s_QAAAAAAAAAAaJCOKiGz7InW9DRHAs24psZYFR4xx1f18nWBPeqlne5";
        
        private const string FilesRequesBody =@"{
    'include_deleted': false,
    'include_has_explicit_shared_members': false,
    'include_media_info': false,
    'include_mounted_folders': false,
    'include_non_downloadable_files': false,
    'path': '',
    'recursive': true
}";


        private static string _tempRuntimeToken = null;

#if UNITY_EDITOR
        // First, call this method to get an authCode, then paste it in the appropriate field above.
        public static void GetAuthCode()
        {
            var url = $"https://www.dropbox.com/oauth2/authorize?client_id={AppKey}&response_type=code&token_access_type=offline";
            Application.OpenURL(url);
        }

        // After you have pasted an AuthCode, call this method to get refreshToken.
        public static async void GetRefreshToken()
        {
            Debug.Log("Requesting refreshToken...");

            var form = new WWWForm();
            form.AddField("code", AuthCode);
            form.AddField("grant_type", "authorization_code");

            var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppKey}:{AppSecret}"));

            using var request = UnityWebRequest.Post("https://api.dropbox.com/oauth2/token", form);
            request.SetRequestHeader("Authorization", $"Basic {base64Authorization}");

            var sendRequest = request.SendWebRequest();

            while (!sendRequest.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                Debug.LogError(request.downloadHandler.text);
                return;
            }
            
            var parsedAnswer = JObject.Parse(request.downloadHandler.text);
            var refreshTokenString = parsedAnswer["refresh_token"]?.Value<string>();

           Debug.Log("Copy this string to RefreshToken: " + refreshTokenString);
        }
#endif

        /// <summary>
        /// Call initialization before you start download any files and await it's completion.
        /// To wait inside a coroutine you can use:
        /// var task = DropboxHelper.Initialize();
        /// yield return new WaitUntil(() => task.IsCompleted);
        /// </summary>
        public static async Task Initialize()
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                Debug.LogError("refreshToken should be defined before runtime");
            }

            var form = new WWWForm();
            form.AddField("grant_type", "refresh_token");
            form.AddField("refresh_token", RefreshToken);

            var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppKey}:{AppSecret}"));

            using var request = UnityWebRequest.Post("https://api.dropbox.com/oauth2/token", form);
            request.SetRequestHeader("Authorization", $"Basic {base64Authorization}");

            var sendRequest = request.SendWebRequest();

            while (!sendRequest.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                Debug.LogError(request.downloadHandler.text);
            }

            Debug.Log("Success! Full message from dropbox: " + request.downloadHandler.text);

           var data = JObject.Parse(request.downloadHandler.text);
           _tempRuntimeToken = data["access_token"]?.Value<string>();

            Debug.Log("Token: " + _tempRuntimeToken);
        }

        /// <summary>
        /// Creating a request for file download.
        /// To wait inside a coroutine you can use:
        /// var task = DropboxHelper.GetRequestForFileDownload();
        /// yield return new WaitUntil(() => task.IsCompleted);
        /// </summary>
        /// <param name="relativePathToFile">Pass a relative path from a root folder. Example: "images/image1"</param>
        /// <returns>WebRequest that you should send and then process it's result</returns>
        public static UnityWebRequest GetRequestForFileDownload(string relativePathToFile)
        {
            var request = UnityWebRequest.Get("https://content.dropboxapi.com/2/files/download");
            request.SetRequestHeader("Authorization", $"Bearer {_tempRuntimeToken}");
            request.SetRequestHeader("Dropbox-API-Arg", $"{{\"path\": \"{relativePathToFile}\"}}");
            return request;
        }
        
        public static async Task DownloadAndSaveFile(string relativePathToFile)
        {
            // Create a download request
            UnityWebRequest downloadRequest = GetRequestForFileDownload(relativePathToFile);

            // Send the download request
            var operation = downloadRequest.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Delay(100); // Wait until the request is completed
            }

            if (downloadRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download file: " + downloadRequest.error);
            }
            else
            {
                // Save the downloaded file to the persistent data path
                string filePath = Application.persistentDataPath + "/" + relativePathToFile;
                System.IO.File.WriteAllBytes(filePath, downloadRequest.downloadHandler.data);
                Debug.Log("File saved to: " + filePath);
            }
        }

        public static async Task<string> GetFiles(string relativePathToFile)
        {
            UnityWebRequest getFilesRequest = GetRequestForFileData(relativePathToFile);
            
            var operation = getFilesRequest.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Delay(100); // Wait until the request is completed
            }

            if (getFilesRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download file: " + getFilesRequest.error);
            }
            else
            {
                Debug.Log("Cool");
                return getFilesRequest.downloadHandler.text;
                // Save the downloaded file to the persistent data path
                // string filePath = Application.persistentDataPath + "/" + relativePathToFile;
                //System.IO.File.WriteAllBytes(filePath, getFilesRequest.downloadHandler.data);
                //Debug.Log("File saved to: " + filePath);
            }

            return string.Empty;
        }
        public static UnityWebRequest GetRequestForFileData(string relativePathToFile)
        {
            var request = new UnityWebRequest("https://api.dropboxapi.com/2/files/list_folder","POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(GetFileDataBody(""));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {_tempRuntimeToken}");
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        public static string GetFileDataBody(string path)
        {
            // Initialize parameters
            bool includeDeleted = false;
            bool includeExplicitSharedMembers = false;
            bool includeMediaInfo = false;
            bool includeMountedFolders = true;
            bool includeNonDownloadableFiles = false;
            bool recursive = true;

            // Construct JSON data for the request body
            var requestData = new
            {
                path,
                include_deleted = includeDeleted,
                include_has_explicit_shared_members = includeExplicitSharedMembers,
                include_media_info = includeMediaInfo,
                include_mounted_folders = includeMountedFolders,
                include_non_downloadable_files = includeNonDownloadableFiles,
                recursive
            };

            // Serialize the object to JSON string
            string jsonData = JsonConvert.SerializeObject(requestData);
            return jsonData;
        }
    }
}