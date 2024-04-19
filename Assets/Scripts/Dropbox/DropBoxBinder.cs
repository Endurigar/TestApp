using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DropBoxBinder : MonoInstaller
{
    [SerializeField] private DropboxHandler dropboxHandler;
    [SerializeField] private DownloadHandler downloadHandler;
    public override void InstallBindings()
    {
        Container.BindInstance(dropboxHandler).AsSingle();
        Container.BindInstance(downloadHandler).AsSingle();
    }
}
