using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterItem : MonoBehaviour
    {
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private TextMeshProUGUI _characterName;
        
        private Button _button;
        public Button Button => _button ? _button : _button = GetComponent<Button>();
        
        private int _characterId;

        public void Setup(GameObject character, string characterName, int characterId)
        {
            Button.onClick.RemoveAllListeners();
            
            if (LocalDatabase.lastSelectedCharacter.Value == characterId)
                Button.Select();
            
            _characterName.text = characterName;
            _characterId = characterId;
            
            //Clear old gameObject
            for (int i = 0; i < _characterRoot.childCount; i++)
                Destroy(_characterRoot.GetChild(i).gameObject);

            Instantiate(character, _characterRoot);
            
            Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LocalDatabase.lastSelectedCharacter.Value = _characterId;
        }
    }
}