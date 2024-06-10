using System;
using System.Threading.Tasks;
using Network.PlayFab;
using TMPro;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace UI.InGame
{
    public class WoodCounter : SingletonBehavior<WoodCounter>
    {
        public Action<int> OnWoodChanged;
        
        private TextMeshProUGUI _woodText;
        private TextMeshProUGUI WoodText => _woodText ? _woodText : (_woodText = GetComponentInChildren<TextMeshProUGUI>());

        private void Awake()
        {
            UpdateWoodText(0);
            OnWoodChanged ??= wood => {};
            OnWoodChanged += UpdateWoodText;
        }

        private async void UpdateWoodText(int wood)
        {
            Database.collectedWoods += wood;
            WoodText.text = $"Wood: {Database.collectedWoods}";
            
            if (wood == 0)
                return;
            
            await Database.SetStats();
        }
    }
}