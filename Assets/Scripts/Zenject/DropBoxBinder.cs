using Dropbox;
using UI.Handlers;
using UnityEngine;

namespace Zenject
{
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
}
