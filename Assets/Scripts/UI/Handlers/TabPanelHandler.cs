using UnityEngine;
using UnityEngine.UI;

namespace UI.Handlers
{
    public class TabPanelHandler : MonoBehaviour
    {
        private Button[] buttons;

        private void Start()
        {
            buttons = gameObject.GetComponentsInChildren<Button>();
        }
    }
}
