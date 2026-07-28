using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ResidentDirectoryController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField]
    private GameObject directoryPanel;

    [Header("Profile Display")]
    [SerializeField]
    private TMP_Text residentNameText;

    [SerializeField]
    private Image portraitImage;

    [SerializeField]
    private TMP_Text portraitPlaceholderText;

    [SerializeField]
    private TMP_Text profileDetailsText;

    [Header("Knowledge")]
    [SerializeField]
    private ResidentKnowledgeManager knowledgeManager;

    [Header("Resident Data")]
    [SerializeField]
    private List<ResidentProfileData> residents =
        new List<ResidentProfileData>();

    private int currentResidentIndex;
    private ResidentKnowledgeManager subscribedKnowledgeManager;

    private void Awake()
    {
        if (directoryPanel != null)
            directoryPanel.SetActive(false);
    }

    private void OnEnable()
    {
        SubscribeToKnowledgeManager();
    }

    private void OnDisable()
    {
        UnsubscribeFromKnowledgeManager();
    }

    public void OpenDirectory()
    {
        if (directoryPanel == null)
        {
            Debug.LogError(
                "The Resident Directory Panel is not assigned.",
                this
            );

            return;
        }

        if (residents.Count == 0)
        {
            Debug.LogError(
                "The Resident Directory does not contain any profiles.",
                this
            );

            return;
        }

        directoryPanel.SetActive(true);

        ShowResident(0);
    }

    public void CloseDirectory()
    {
        if (directoryPanel != null)
            directoryPanel.SetActive(false);
    }

    public void ShowResident(int residentIndex)
    {
        if (residentIndex < 0 ||
            residentIndex >= residents.Count)
        {
            Debug.LogError(
                $"Resident index {residentIndex} is invalid.",
                this
            );

            return;
        }

        ResidentProfileData resident =
            residents[residentIndex];

        if (resident == null)
        {
            Debug.LogError(
                $"Resident profile {residentIndex} is missing.",
                this
            );

            return;
        }

        currentResidentIndex =
            residentIndex;

        if (residentNameText != null)
        {
            residentNameText.text =
                resident.DisplayName;
        }

        bool hasPortrait =
            resident.Portrait != null;

        if (portraitImage != null)
        {
            portraitImage.sprite =
                resident.Portrait;

            portraitImage.gameObject.SetActive(
                hasPortrait
            );

            portraitImage.preserveAspect = true;
        }

        if (portraitPlaceholderText != null)
        {
            portraitPlaceholderText.gameObject.SetActive(
                !hasPortrait
            );
        }

        if (profileDetailsText != null)
        {
            profileDetailsText.text =
                BuildProfileText(resident);
        }
    }

    private string BuildProfileText(
        ResidentProfileData resident)
    {
        ResidentKnowledgeManager activeKnowledgeManager =
            GetKnowledgeManager();

        if (activeKnowledgeManager == null ||
            !activeKnowledgeManager.HasKnowledgeNotes)
        {
            return BuildFullStaticProfileText(
                resident
            );
        }

        return BuildProgressiveProfileText(
            resident,
            activeKnowledgeManager
        );
    }

    private string BuildFullStaticProfileText(
        ResidentProfileData resident)
    {
        return
            $"<b>PROFILE</b>\n" +
            $"{resident.Summary}\n\n" +

            $"<b>ADDRESS</b>\n" +
            $"{resident.Address}\n\n" +

            $"<b>ALIASES AND OTHER NAMES</b>\n" +
            $"{resident.Aliases}\n\n" +

            $"<b>LIKES</b>\n" +
            $"{resident.Likes}\n\n" +

            $"<b>DISLIKES</b>\n" +
            $"{resident.Dislikes}\n\n" +

            $"<b>DELIVERY NOTES</b>\n" +
            $"{resident.DeliveryNotes}";
    }

    private string BuildProgressiveProfileText(
        ResidentProfileData resident,
        ResidentKnowledgeManager activeKnowledgeManager)
    {
        StringBuilder profileText =
            new StringBuilder();

        profileText
            .Append("<b>PROFILE</b>\n")
            .Append(GetDisplayValue(
                resident.Summary,
                "No profile details recorded."
            ))
            .Append("\n\n")
            .Append("<b>ADDRESS</b>\n")
            .Append(GetDisplayValue(
                resident.Address,
                "No address recorded."
            ))
            .Append("\n\n")
            .Append("<b>ALIASES AND OTHER NAMES</b>\n")
            .Append(GetDisplayValue(
                resident.Aliases,
                "No aliases recorded."
            ))
            .Append("\n\n")
            .Append("<b>LIKES</b>\n")
            .Append(GetDisplayValue(
                resident.Likes,
                "No likes recorded."
            ))
            .Append("\n\n")
            .Append("<b>DISLIKES</b>\n")
            .Append(GetDisplayValue(
                resident.Dislikes,
                "No dislikes recorded."
            ))
            .Append("\n\n")
            .Append("<b>DELIVERY NOTES</b>\n")
            .Append(GetDisplayValue(
                resident.DeliveryNotes,
                "No delivery notes recorded."
            ))
            .Append("\n\n")
            .Append("<b>RESIDENT NOTES</b>\n");

        List<ResidentKnowledgeNote> unlockedNotes =
            activeKnowledgeManager
                .GetUnlockedNotesForResident(
                    resident
                );

        if (unlockedNotes.Count == 0)
        {
            profileText.Append(
                "No extra resident notes discovered yet."
            );

            return profileText.ToString();
        }

        foreach (ResidentKnowledgeNote note in unlockedNotes)
        {
            if (note == null ||
                string.IsNullOrWhiteSpace(note.DisplayText))
            {
                continue;
            }

            profileText
                .Append("- ")
                .Append(note.DisplayText)
                .Append('\n');
        }

        return profileText.ToString().TrimEnd();
    }

    private void HandleNoteUnlocked(
        ResidentKnowledgeNote note)
    {
        if (directoryPanel == null ||
            !directoryPanel.activeSelf)
        {
            return;
        }

        if (currentResidentIndex < 0 ||
            currentResidentIndex >= residents.Count)
        {
            return;
        }

        ResidentProfileData currentResident =
            residents[currentResidentIndex];

        if (note == null ||
            !note.BelongsToResident(currentResident))
        {
            return;
        }

        ShowResident(currentResidentIndex);
    }

    private ResidentKnowledgeManager GetKnowledgeManager()
    {
        if (knowledgeManager == null)
        {
            knowledgeManager =
                UnityEngine.Object
                    .FindFirstObjectByType<ResidentKnowledgeManager>();
        }

        SubscribeToKnowledgeManager();

        return knowledgeManager;
    }

    private void SubscribeToKnowledgeManager()
    {
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

    private string GetDisplayValue(
        string value,
        string fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
            return fallback;

        return value;
    }
}
