using UnityEngine;
using Zenject;

public class UIBinder : MonoInstaller
{
    [SerializeField] private LoadingBar loadingBar;
    [SerializeField] private DownloadFailHandler downloadFailHandler;
    public override void InstallBindings()
    {
        Container.BindInstance(loadingBar).AsSingle();
        Container.BindInstance(downloadFailHandler).AsSingle();
    }
}