using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ResidentKnowledgeNotificationUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private ResidentKnowledgeManager knowledgeManager;

    [SerializeField]
    private GameObject notificationPanel;

    [SerializeField]
    private TMP_Text headerText;

    [SerializeField]
    private TMP_Text residentNameText;

    [SerializeField]
    private TMP_Text noteText;

    [SerializeField]
    private Button dismissButton;

    [Header("Timing")]
    [SerializeField]
    private float displaySeconds = 3.0f;

    [SerializeField]
    private string headerMessage =
        "NEW RESIDENT NOTE DISCOVERED";

    private Coroutine hideRoutine;
    private ResidentKnowledgeManager subscribedKnowledgeManager;

    private void Awake()
    {
        HideNotification();
    }

    private void OnEnable()
    {
        SubscribeToKnowledgeManager();

        if (dismissButton != null)
            dismissButton.onClick.AddListener(Dismiss);
    }

    private void OnDisable()
    {
        UnsubscribeFromKnowledgeManager();

        if (dismissButton != null)
            dismissButton.onClick.RemoveListener(Dismiss);
    }

    public void Dismiss()
    {
        HideNotification();
    }

    private void HandleNoteUnlocked(
        ResidentKnowledgeNote note)
    {
        if (note == null)
            return;

        if (notificationPanel == null)
        {
            Debug.LogWarning(
                "Resident knowledge notification panel is not assigned.",
                this
            );

            return;
        }

        if (headerText != null)
            headerText.text = headerMessage;

        if (residentNameText != null)
            residentNameText.text = note.ResidentDisplayName;

        if (noteText != null)
            noteText.text = note.DisplayText;

        notificationPanel.SetActive(true);

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

        HideNotification();
    }

    private void HideNotification()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    private void SubscribeToKnowledgeManager()
    {
        if (knowledgeManager == null)
        {
            knowledgeManager =
                UnityEngine.Object
                    .FindFirstObjectByType<ResidentKnowledgeManager>();
        }

        if (knowledgeManager == null ||
            subscribedKnowledgeManager == knowledgeManager)
        {
            return;
        }

        UnsubscribeFromKnowledgeManager();

        subscribedKnowledgeManager = knowledgeManager;
        subscribedKnowledgeManager.NoteUnlocked += HandleNoteUnlocked;
    }

    private void UnsubscribeFromKnowledgeManager()
    {
        if (subscribedKnowledgeManager == null)
            return;

        subscribedKnowledgeManager.NoteUnlocked -= HandleNoteUnlocked;
        subscribedKnowledgeManager = null;
    }
}
