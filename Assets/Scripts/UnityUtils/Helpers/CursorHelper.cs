using UnityEngine;
using UnityUtils.StaticClasses;

namespace UnityUtils.Helpers
{
    public class CursorHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool IsUIState
        {
            get
            {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            return true;
#else
                return Cursor.visible && Cursor.lockState != CursorLockMode.Locked;
#endif
            }
        }
        
        /// <summary>
        /// Helper for Cursor locked in Unity 5
        /// </summary>
        /// <param name="mLock">cursor state</param>
        public static void LockCursor(bool mLock)
        {
            //if (BlockCursorForUser) return;
            if (mLock)
            {
                CursorLockMode cm = Platform.IsMobile ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = false;
                Cursor.lockState = cm;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
        /// <summary>
        /// Helper for Cursor locked in Unity 5
        /// </summary>
        /// <param name="mLock">cursor state</param>
        public static void ChangeCursor() => LockCursor(IsUIState);
        
    }
}