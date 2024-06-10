using System.Collections.Generic;
using System.Linq;
using Config;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using InGame.Player;
using Network;
using Network.Photon;
using Photon.Pun;
using UI.Base;
using UnityEngine;
using UnityUtils.Extensions;

namespace UI.InGame
{
    public class SelectColorPanel : BasePanel
    {
        [SerializeField] private ColorItem _colorItemReference;
        [SerializeField] private List<ColorItem> _colorItems = new List<ColorItem>();
        protected override void Start()
        {
            base.Start();
            
            foreach (var color in PlayerColorPalette.Colors)
            {
                var colorItem = Instantiate(_colorItemReference, _colorItemReference.transform.parent);
                colorItem.Setup(color);
                _colorItems.Add(colorItem);
            }
            _colorItemReference.SetActivate(false);
        }

        public override TweenerCore<float, float, FloatOptions> ShowPanel()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                foreach (var colorItem in _colorItems)
                {
                    colorItem.Activate(player.GetPlayerColorIndex() != colorItem.colorIndex);
                }
            }
            return base.ShowPanel();
        }
            
    }
}