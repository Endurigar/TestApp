using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
