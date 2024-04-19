using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InternetConnectionCheck : Page
{
    [SerializeField] private Button retryButton;
    private bool internetAvailable = true;
    private WaitForSeconds waitTime = new WaitForSeconds(1);
    private void Start()
    {
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
