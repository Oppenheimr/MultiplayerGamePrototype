using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;

namespace UI.Base
{
    public class PanelManager : SingletonBehavior<PanelManager>
    {
        [SerializeField] private BasePanel[] _panels;
        [SerializeField] private BasePanel _activePanel;
        [SerializeField] private BasePanel _previousPanel;
        
        private bool _inProgress;
        
        protected void Start()
        {
            foreach (var panel in _panels)
                panel.SetActivate(false);
            
            ShowPanel(_activePanel);
        }

        public virtual TweenerCore<float, float, FloatOptions> ShowPanel(BasePanel panel)
        {
            if (_inProgress)
                return null;
            
            return _activePanel.HidePanel().OnStart(() => { _inProgress = true;}).OnComplete(() => {
                _inProgress = false;
                TabNavigation.Instance.ResetLastSelected();
                _previousPanel = _activePanel;
                _activePanel = panel;
                _previousPanel.SetActivate(false);
                _activePanel.SetActivate(true);
                panel.ShowPanel();
            });
        }
        
        public virtual TweenerCore<float, float, FloatOptions> ShowPanel(int index) =>
            _activePanel.HidePanel().OnComplete(() => { _activePanel = _panels[index]; _panels[index].ShowPanel(); });

        public void ShowNextPanel()
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                if (_panels[i] != _activePanel)
                    continue;

                ShowPanel(_panels.Length <= i + 1 ? _panels[0] : _panels[i + 1]);
                
                break;
            }
        }
        
        public void ShowPanel(Type panel, bool forcedProcess = false)
        {
            if (forcedProcess)
                _inProgress = false;
            
            foreach (var p in _panels)
            {
                if (p.GetType() != panel)
                    continue;
                
                ShowPanel(p);
                break;
            }
        }

        public void ShowPreviousPanel() => ShowPanel(_activePanel.previousPanel);
    }
}