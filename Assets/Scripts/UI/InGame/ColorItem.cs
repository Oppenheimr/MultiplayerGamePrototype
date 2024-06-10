using Config;
using Managers;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Extensions;

namespace UI.InGame
{
    public class ColorItem : MonoBehaviour
    {
        [HideInInspector] public int colorIndex;
        
        [SerializeField] private Image _colorImage;

        private TextMeshProUGUI _text;
        private TextMeshProUGUI Text => _text ? _text : (_text = GetComponentInChildren<TextMeshProUGUI>());
        
        private Button _button;
        private Button Button => _button ? _button : (_button = GetComponent<Button>());
        
        private bool _active;

        public void OnSelectColor()
        {
            if (!_active) 
                return;
            
            GameManager.Instance.PlayerColorController.SyncColor(_colorImage.color);
            Activate(false);
        }

        public void Setup(Color color)
        {
            colorIndex = PlayerColorPalette.GetColorIndex(color);
            _colorImage.color = color;
            Activate(true);
        }
        
        public void Activate(bool active)
        {
            _active = active;
            Text.text = active ? "Select" : "Used by another player";
            _colorImage.color = _colorImage.color.SetAlpha(active ? 1 : 0.5f);
            
            Button.interactable = active;
        }
    }
}