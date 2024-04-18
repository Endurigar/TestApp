using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;


    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Page : MonoBehaviour
    {
        [SerializeField]private CanvasGroup canvasGroup;
        [SerializeField] private float pageFadeDuration;
        private bool isShowed;

        public bool IsShowed => isShowed;

        protected virtual void Start()
        {
            Init();
        }

        public virtual void Show()
        {
            canvasGroup.DOFade(1, pageFadeDuration);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            isShowed = true;
        }

        public virtual void Hide()
        {
            canvasGroup.DOFade(0, pageFadeDuration);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            isShowed = false;
        }

        protected virtual void Init()
        {
            if(canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }
    }
