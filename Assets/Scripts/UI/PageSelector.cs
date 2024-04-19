using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PageSelector : MonoBehaviour
    {
        [SerializeField] private Page page;

        public Page Page
        {
            get => page;
        }

        private Button button;

        private void Start()
        {
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(SelectPage);
        }

        private void SelectPage()
        {
            page.Show();
        }
    }
}
