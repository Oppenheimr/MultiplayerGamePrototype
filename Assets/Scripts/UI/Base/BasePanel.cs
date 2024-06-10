using Core;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils.Extensions;

namespace UI.Base
{
    public class BasePanel : MonoBehaviour
    {
        [FormerlySerializedAs("_previousPanel")]
        public BasePanel previousPanel;
        
        [Header("Animation")]
        [SerializeField] protected float _animTime = 1.2f;
        [SerializeField] protected bool _startTest;
        
        protected const float _fullTransparency = 0;
        protected const float _fullOpacity = 1;
        protected Vector3 _startPos;

        private CanvasGroup _canvasGroup;
        public CanvasGroup Group => _canvasGroup ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>());

        protected virtual void Start()
        {
            _startPos = transform.localPosition;
            Group.alpha = _fullTransparency;
            if (_startTest)
                ShowPanel();
        }

        public void Show() => ShowPanel();
        public void Hide() => HidePanel();

        public virtual TweenerCore<float, float, FloatOptions> ShowPanel()
        {
            transform.DOLocalMove(Vector3.zero, _animTime);
            return Group.DOFade(_fullOpacity, _animTime);
        }

        public virtual TweenerCore<float, float, FloatOptions> HidePanel()
        {
            transform.DOLocalMove(_startPos, _animTime);
            return Group.DOFade(_fullTransparency, _animTime);
        }
    }

    /// <summary>
    /// Türetilecek tim sınıflara "Ins" tanımı ekler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePanel<T> : BasePanel where T : BasePanel, new()
    {
        private static T instance;
        public static T Instance => instance ? instance : (instance = FindObjectOfType<T>(true));
    }
}