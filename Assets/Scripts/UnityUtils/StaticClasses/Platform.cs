namespace UnityUtils.StaticClasses
{
    public static class Platform
    {
        private static bool testMode;
        private static PlatformType testType;

        public static void TestEnable(PlatformType type)
        {
            testMode = true;
            testType = type;
        }

        public static void TestDisable() => testMode = false;
        
        public static bool IsEditor => GetPlatform() == PlatformType.Editor;
        public static bool IsWeb => GetPlatform() == PlatformType.Web;
        public static bool IsDesktop => GetPlatform() == PlatformType.Desktop;
        public static bool IsMobile => GetPlatform() == PlatformType.Mobile;
        
        public static PlatformType GetPlatform()
        {
            if (testMode)
                return testType;
            
#if UNITY_EDITOR
            return PlatformType.Editor;
#elif UNITY_ANDROID || UNITY_IOS || UNITY_ANDROID_API || PLATFORM_ANDROID
            return PlatformType.Mobile;
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WSA
            return PlatformType.Desktop;
#elif UNITY_WEBGL
            return PlatformType.Web;
#endif
        }
    }

    public enum PlatformType
    {
        All,
        Editor,
        Web,
        Desktop,
        Mobile,
    }
}