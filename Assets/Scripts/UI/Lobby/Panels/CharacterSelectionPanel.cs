using Data;
using UI.Base;
using UnityEngine;

namespace UI.Lobby.Panels
{
    public class CharacterSelectionPanel : BasePanel
    {
        [SerializeField] private CharacterItem[] _items;

        protected override void Start()
        {
            base.Start();
            
            for (int i = 0; i < _items.Length; i++)
            {
                if (GameData.Instance.characters.Length < i + 1)
                    break;

                var character = GameData.Instance.characters[i];

                if (character == null)
                    continue;
                
                if (_items[i] == null)
                    continue;
                
                _items[i].Setup(character.showModel, character.name, i);
            }
        }
        
        public void OnSelectCharacter(int index)
        {
            PanelManager.Instance.ShowPanel(typeof(ConnectedPanel));
        }
    }
}