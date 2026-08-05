using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseOverlay;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private Button resumeButton;

    private float timeScaleBeforePause = 1.0f;
    private bool isPaused;

    private void Awake()
    {
        HideOverlay();
    }

    private void OnEnable()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(Pause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
    }

    private void OnDisable()
    {
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(Pause);

        if (resumeButton != null)
            resumeButton.onClick.RemoveListener(Resume);

        if (isPaused)
            Resume();
    }

    public void Pause()
    {
        if (isPaused)
            return;

        timeScaleBeforePause =
            Time.timeScale > 0.0f
                ? Time.timeScale
                : 1.0f;

        isPaused = true;
        Time.timeScale = 0.0f;

        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);
    }

    public void Resume()
    {
        if (!isPaused)
        {
            HideOverlay();
            return;
        }

        Time.timeScale =
            Mathf.Approximately(timeScaleBeforePause, 0.0f)
                ? 1.0f
                : timeScaleBeforePause;

        isPaused = false;
        HideOverlay();
    }

    private void HideOverlay()
    {
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);
    }
}
