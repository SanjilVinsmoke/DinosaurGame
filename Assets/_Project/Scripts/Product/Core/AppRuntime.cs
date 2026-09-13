using UnityEngine;

namespace DinosaurGame.Core
{
    public static class AppRuntime
    {
        public const int TargetFrameRate = 60;

        public static void Configure()
        {
            Application.targetFrameRate = TargetFrameRate;
            Input.multiTouchEnabled = true;
        }

        public static bool Require(Object value, string label, Object context)
        {
            if (value != null) return true;
            Debug.LogError($"[Bootstrap] Missing required reference: {label}", context);
            return false;
        }
    }
}
