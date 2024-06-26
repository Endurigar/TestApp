using TMPro;
using UI.Pages;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UiUtilities
{
    public class TintText : MonoBehaviour
    {
        private Button button;
        private TMP_Text text;
        private Page page;

        private void Start()
        {
            page = gameObject.GetComponent<PageSelector>().Page;
            button = gameObject.GetComponent<Button>();
            text = gameObject.GetComponentInChildren<TMP_Text>();
            page.OnPageShow += OnPageShow;
            page.OnPageHide += OnPageHided;
        }

        private void OnPageShow(Page obj)
        {
            button.targetGraphic.color = button.colors.selectedColor;
            text.color = button.colors.selectedColor;
        }

        private void OnPageHided(Page obj)
        {
            button.targetGraphic.color = button.colors.normalColor;
            text.color = button.colors.normalColor;
        }
    }
}
