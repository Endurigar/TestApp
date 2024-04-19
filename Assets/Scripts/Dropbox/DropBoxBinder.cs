using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DropBoxBinder : MonoInstaller
{
    [SerializeField] private DropboxHandler dropboxHandler;
    public override void InstallBindings()
    {
        Container.BindInstance(dropboxHandler).AsSingle();
    }
}
