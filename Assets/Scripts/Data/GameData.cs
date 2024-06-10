using Photon.Pun;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(GameData), menuName = "Scriptables/Game Data", order = 1)]
    public class GameData : SingletonScriptable<GameData>
    {
        public CharacterData[] characters;
        
        public string GetSelectedCharacterName() => characters[LocalDatabase.lastSelectedCharacter.Value].name;
    }
}