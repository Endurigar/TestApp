using UnityEngine;
using UnityEngine.UI;

namespace UI.UiUtilities
{
    public class CategoryButton : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = gameObject.GetComponent<Button>();
        }
    }
}
