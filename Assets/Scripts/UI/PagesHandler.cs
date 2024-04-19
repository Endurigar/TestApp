using System;
using UnityEngine;

namespace UI
{
    public class PagesHandler : MonoBehaviour
    {
        private Page[] pages;
        private Page latestPage;

        private void Start()
        {
            pages = gameObject.GetComponentsInChildren<Page>();
            foreach (var page in pages)
            {
                page.OnPageShow += OnPageStateChanged;
            }
        }

        private void OnPageStateChanged(Page page)
        {
            if (latestPage != null)
            {
                latestPage.Hide();
            }
            latestPage = page;
        }
    }
}
