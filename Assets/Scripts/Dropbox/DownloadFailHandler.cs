using System.Collections;
using UI.Pages;
using UnityEngine;

namespace Dropbox
{
    public class DownloadFailHandler : Page
    {
        public override void Show()
        {
            base.Show();
            StartCoroutine(DelayForFade());
        }

        private IEnumerator DelayForFade()
        {
            yield return new WaitForSeconds(1f);
            Hide();
        }
    }
}
