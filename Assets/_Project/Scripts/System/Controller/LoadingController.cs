using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField, Min(0f)] private float minimumDisplayTime = 0.35f;
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("Components")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start()
    {
        StartCoroutine(LoadGameplay());
    }

    private IEnumerator LoadGameplay()
    {
        if (string.IsNullOrWhiteSpace(gameplaySceneName) || !Application.CanStreamedLevelBeLoaded(gameplaySceneName))
        {
            Debug.LogError($"[Bootstrap] Gameplay scene '{gameplaySceneName}' is not available in Build Settings.", this);
            yield break;
        }

        var startedAt = Time.realtimeSinceStartup;
        var operation = SceneManager.LoadSceneAsync(gameplaySceneName);
        if (operation == null)
        {
            Debug.LogError($"[Bootstrap] Failed to start loading scene '{gameplaySceneName}'.", this);
            yield break;
        }

        operation.allowSceneActivation = false;
        SetProgress(0f);

        while (!operation.isDone)
        {
            var sceneProgress = Mathf.Clamp01(operation.progress / 0.9f);
            var timeProgress = minimumDisplayTime <= 0f
                ? 1f
                : Mathf.Clamp01((Time.realtimeSinceStartup - startedAt) / minimumDisplayTime);
            var displayedProgress = Mathf.Min(sceneProgress, timeProgress);
            SetProgress(displayedProgress);

            if (operation.progress >= 0.9f && timeProgress >= 1f)
            {
                SetProgress(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void SetProgress(float normalized)
    {
        if (slider != null)
            slider.normalizedValue = normalized;
        if (loadingText != null)
            loadingText.text = $"Loading {Mathf.RoundToInt(normalized * 100f)}%";
    }

    public void OnSliderValueChanged()
    {
        if (slider != null && loadingText != null)
            loadingText.text = $"Loading {Mathf.RoundToInt(slider.normalizedValue * 100f)}%";
    }
}
