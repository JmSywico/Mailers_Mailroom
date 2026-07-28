using System.Collections;
using TMPro;
using UnityEngine;

public sealed class MailDeliveryAttemptFeedbackUI : MonoBehaviour
{
    [SerializeField]
    private GameObject feedbackPanel;

    [SerializeField]
    private TMP_Text feedbackText;

    [SerializeField]
    private float displaySeconds = 1.4f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        HideMessage();
    }

    public void ShowMessage(
        string message)
    {
        if (feedbackPanel == null)
        {
            Debug.Log(
                message,
                this
            );

            return;
        }

        if (feedbackText != null)
        {
            feedbackText.text =
                string.IsNullOrWhiteSpace(message)
                    ? "Take another look."
                    : message;
        }

        feedbackPanel.SetActive(true);

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine =
            StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(
            Mathf.Max(0.1f, displaySeconds)
        );

        hideRoutine = null;

        HideMessage();
    }

    private void HideMessage()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }
}
