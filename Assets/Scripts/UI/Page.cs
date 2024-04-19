using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;


    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Page : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string pageName;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float pageFadeDuration;
        public Action<Page> OnPageShow;
        public Action<Page> OnPageHide;
        private bool isShowed;

        public bool IsShowed => isShowed;

        protected virtual void Start()
        {
            if (text != null)
            {
                text.text = pageName;
            }
            Init();
        }

        public virtual void Show()
        {
            OnPageShow?.Invoke(this);
            canvasGroup.DOFade(1, pageFadeDuration);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            isShowed = true;
        }

        public virtual void Hide()
        {
            OnPageHide?.Invoke(this);
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
