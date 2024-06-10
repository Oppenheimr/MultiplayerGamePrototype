using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUtils.BaseClasses;

namespace UI
{
    public class TabNavigation : SingletonBehavior<TabNavigation>
    {
        private List<Selectable> _selectables;
        private int _currentIndex = 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Sahnedeki tüm Selectable bileşenleri topla
                _selectables = new List<Selectable>(FindObjectsOfType<Selectable>(false));
                // Sıralı gezinme için listeyi sıralayın
                _selectables.Sort((x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));

                // Bir sonraki selectable'a geç
                _currentIndex = (_currentIndex + 1) % _selectables.Count;
                EventSystem.current.SetSelectedGameObject(_selectables[_currentIndex].gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // Enter tuşuna basıldığında seçili butonu tetikleyin
                var selected = EventSystem.current.currentSelectedGameObject;
                if (selected != null)
                {
                    var button = selected.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke();
                    }
                }
            }
        }
        
        public void ResetLastSelected() => _currentIndex = 0;
    }
}