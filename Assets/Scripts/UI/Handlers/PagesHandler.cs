using UI.Pages;
using UnityEngine;

namespace UI.Handlers
{
    public class PagesHandler : MonoBehaviour
    {
        private Page[] pages;
        private Page latestPage;

        private void Start()
        {
            pages = gameObject.GetComponentsInChildren<MenuPages>();
            foreach (var page in pages)
            {
                page.OnPageShow += OnPageStateChanged;
            }
        }

        private void OnPageStateChanged(Page page)
        {
            if (latestPage != null) latestPage.Hide();
            latestPage = page;
        }
    }
}
