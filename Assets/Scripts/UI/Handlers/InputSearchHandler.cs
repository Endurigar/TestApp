using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Handlers
{
    public class InputSearchHandler : MonoBehaviour
    {
        [SerializeField] private Button crossButton;
        public Action<string> OnValueChanged;

        public TMP_InputField Field { get; private set; }

        private void Start()
        {
            Field = gameObject.GetComponent<TMP_InputField>();
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
