using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InputSearchHandler : MonoBehaviour
    {
        [SerializeField] private Button crossButton;
        public Action<string> OnValueChanged;
        private TMP_InputField inputField;

        public TMP_InputField Field => inputField;

        private void Start()
        {
            inputField = gameObject.GetComponent<TMP_InputField>();
            Field.onValueChanged.AddListener(ValueChanged);
            crossButton.onClick.AddListener(ClearInputField);
        }

        private void ValueChanged(string value)
        {
            OnValueChanged(value);
            crossButton.gameObject.SetActive(value != string.Empty);
        }

        private void ClearInputField()
        {
            Field.text = string.Empty;
        }
    }
}
