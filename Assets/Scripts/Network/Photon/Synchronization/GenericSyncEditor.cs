using UnityEditor;
using UnityEngine;

namespace Network.Photon.Synchronization
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(GenericSync))]
    public class GenericSyncEditor : Editor
    {
        private GenericSync _script;

        private void OnEnable()
        {
            _script = (GenericSync)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (_script.syncMethod != SyncMethods.PhotonRaise)
                return;

            if (_script.raise == null)
                _script.raise = new PhotonRaise(_script);    
            
            //script içinde bulunan raise içinde ki değişkenleri göster
            EditorGUILayout.LabelField("Photon Raise Options", EditorStyles.boldLabel);
            _script.raise.raiseEventFrequency = EditorGUILayout.IntField("Raise Event Frequency", _script.raise.raiseEventFrequency);
            _script.raise.raiseAnimEventFrequency = EditorGUILayout.IntField("Raise Anim Event Frequency", _script.raise.raiseAnimEventFrequency);
            _script.raise.syncPositionSensitivity = EditorGUILayout.FloatField("Sync Position Sensitivity", _script.raise.syncPositionSensitivity);
            _script.raise.syncRotationSensitivity = EditorGUILayout.FloatField("Sync Rotation Sensitivity", _script.raise.syncRotationSensitivity);
        }
    }
#endif
}