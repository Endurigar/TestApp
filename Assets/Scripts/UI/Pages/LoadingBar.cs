using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class LoadingBar : Page
    {
        private const float Duration = 0.5f;
        [SerializeField] private Image bar;

        public void SetBarValue(float value)
        {
            bar.DOFillAmount(value, Duration);
        }

        public void ResetValue()
        {
            bar.fillAmount = 0;
        }
    }
}
