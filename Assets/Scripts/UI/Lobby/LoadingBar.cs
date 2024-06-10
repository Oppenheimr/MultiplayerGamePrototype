using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.Lobby
{
    public class LoadingBar : SingletonBehavior<LoadingBar>
    {
        private TextMeshProUGUI _loadingText;
        private TextMeshProUGUI LoadingText => _loadingText ? _loadingText : (_loadingText = GetComponentInChildren<TextMeshProUGUI>());
        
        private CanvasGroup _canvasGroup;
        private CanvasGroup Group => _canvasGroup ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>());

        private void Awake()
        {
            Group.alpha = 0;
        }

        public void SetLoadingText(string text) => LoadingText.text = text;
        
        public void ShowLoadingBar(string message)
        {
            LoadingText.text = message;
            Group.DOFade(1, 1);
        }
        
        public void HideLoadingBar() => Group.DOFade(0, 1);
        
    }
}