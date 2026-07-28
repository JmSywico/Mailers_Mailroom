using System;
using UnityEngine;

public sealed class MailDeliveryAttemptTracker : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField]
    private MailDeliveryAttemptFeedbackUI feedbackUI;

    [SerializeField]
    private string firstWrongAttemptMessage =
        "This doesn't seem right.";

    [SerializeField]
    private string reinspectionRequiredMessage =
        "Take another look.";

    [Header("Rules")]
    [SerializeField]
    private int wrongAttemptsBeforeReinspection = 2;

    private int consecutiveWrongAttempts;
    private bool requiresReinspection;
    private bool hasShownBlockedDragReminder;

    public int ConsecutiveWrongAttempts =>
        consecutiveWrongAttempts;

    public bool RequiresReinspection =>
        requiresReinspection;

    public bool CanDrag =>
        !requiresReinspection;

    public event Action<MailDeliveryAttemptTracker, string> FeedbackRequested;
    public event Action<MailDeliveryAttemptTracker> ReinspectionRequired;
    public event Action<MailDeliveryAttemptTracker> ReinspectionCleared;

    public void RecordWrongAttempt()
    {
        if (requiresReinspection)
        {
            RequestFeedback(
                reinspectionRequiredMessage
            );

            return;
        }

        consecutiveWrongAttempts++;

        if (consecutiveWrongAttempts >=
            Mathf.Max(1, wrongAttemptsBeforeReinspection))
        {
            requiresReinspection = true;
            hasShownBlockedDragReminder = false;

            RequestFeedback(
                reinspectionRequiredMessage
            );

            ReinspectionRequired?.Invoke(this);

            return;
        }

        RequestFeedback(
            firstWrongAttemptMessage
        );
    }

    public void MarkInspected()
    {
        bool wasReinspectionRequired =
            requiresReinspection;

        consecutiveWrongAttempts = 0;
        requiresReinspection = false;
        hasShownBlockedDragReminder = false;

        if (wasReinspectionRequired)
            ReinspectionCleared?.Invoke(this);
    }

    public void NotifyDragBlocked()
    {
        if (!requiresReinspection)
            return;

        if (hasShownBlockedDragReminder)
            return;

        hasShownBlockedDragReminder = true;

        RequestFeedback(
            reinspectionRequiredMessage
        );
    }

    private void RequestFeedback(
        string message)
    {
        string feedbackMessage =
            string.IsNullOrWhiteSpace(message)
                ? "Take another look."
                : message;

        FeedbackRequested?.Invoke(
            this,
            feedbackMessage
        );

        if (feedbackUI != null)
        {
            feedbackUI.ShowMessage(
                feedbackMessage
            );

            return;
        }

        Debug.Log(
            feedbackMessage,
            this
        );
    }
}
