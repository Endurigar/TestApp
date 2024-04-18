using UnityEngine;
using Zenject;

public class UIBinder : MonoInstaller
{
    [SerializeField] private LoadingBar loadingBar;
    public override void InstallBindings()
    {
        Container.BindInstance(loadingBar).AsSingle();
    }
}