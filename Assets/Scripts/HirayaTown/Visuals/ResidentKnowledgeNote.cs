using System;
using UnityEngine;

[Serializable]
public sealed class ResidentKnowledgeNote
{
    [SerializeField]
    private string noteId;

    [SerializeField]
    private ResidentProfileData residentProfile;

    [TextArea(2, 5)]
    [SerializeField]
    private string displayText;

    [SerializeField]
    private bool beginsUnlocked;

    public string NoteId => noteId;
    public ResidentProfileData ResidentProfile => residentProfile;
    public string DisplayText => displayText;
    public bool BeginsUnlocked => beginsUnlocked;

    public string ResidentDisplayName
    {
        get
        {
            if (residentProfile == null)
                return "Unknown Resident";

            return residentProfile.DisplayName;
        }
    }

    public bool BelongsToResident(
        ResidentProfileData resident)
    {
        if (resident == null ||
            residentProfile == null)
        {
            return false;
        }

        if (residentProfile == resident)
            return true;

        string residentProfileId =
            residentProfile.ResidentId?.Trim();

        string residentId =
            resident.ResidentId?.Trim();

        if (string.IsNullOrWhiteSpace(residentProfileId) ||
            string.IsNullOrWhiteSpace(residentId))
        {
            return false;
        }

        return string.Equals(
            residentProfileId,
            residentId,
            StringComparison.OrdinalIgnoreCase
        );
    }
}
