using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class InternetConnectionCheck : Page
    {
        [SerializeField] private Button retryButton;
        private bool internetAvailable = true;
        private readonly WaitForSeconds waitTime = new(1);

        protected override void Start()
        {
            base.Start();
            retryButton.onClick.AddListener(OnRetryButton);
            StartCoroutine(CheckInternetConnection());
        }

        private IEnumerator CheckInternetConnection()
        {
            while (true)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    if (internetAvailable)
                    {
                        internetAvailable = false;
                        Show();
                    }
                }
                else
                {
                    if (!internetAvailable)
                    {
                        internetAvailable = true;
                        Hide();
                    }
                }

                yield return waitTime;
            }
        }

        private void OnRetryButton()
        {
            Application.Quit();
        }
    }
}
