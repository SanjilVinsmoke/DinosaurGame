using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DinosaurGame.Editor
{
    public static class Epic0ProjectValidator
    {
        [MenuItem("Dinosaur Game/Validate Epic 0 Baseline")]
        public static void Validate()
        {
            var enabledScenes = EditorBuildSettings.scenes.Where(s => s.enabled).ToArray();
            var errors = 0;

            if (!enabledScenes.Any(s => s.path.EndsWith("LoadingScene.unity")))
            {
                Debug.LogError("[Epic0] LoadingScene is not enabled in Build Settings.");
                errors++;
            }

            if (!enabledScenes.Any(s => s.path.EndsWith("GameplayScene.unity") || s.path.EndsWith("GamePlayScene.unity")))
            {
                Debug.LogError("[Epic0] GameplayScene is not enabled in Build Settings.");
                errors++;
            }

            Debug.Log(errors == 0
                ? "[Epic0] Baseline validation passed."
                : $"[Epic0] Baseline validation failed with {errors} issue(s).");
        }
    }
}
