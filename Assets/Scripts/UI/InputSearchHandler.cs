using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InputSearchHandler : MonoBehaviour
    {
        [SerializeField] private Button crossButton;
        private TMP_InputField inputField;

        private void Start()
        {
            inputField = gameObject.GetComponent<TMP_InputField>();
            inputField.onValueChanged.AddListener(OnValueChanged);
            crossButton.onClick.AddListener(ClearInputField);
        }

        private void OnValueChanged(string value)
        {
            crossButton.gameObject.SetActive(value != string.Empty);
        }

        private void ClearInputField()
        {
            inputField.text = string.Empty;
        }
    }
}
