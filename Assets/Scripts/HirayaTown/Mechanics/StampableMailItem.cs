using TMPro;
using UnityEngine;

public sealed class StampableMailItem : MonoBehaviour
{
    [Header("Recipient")]
    [SerializeField]
    private ResidentProfileData recipientProfile;

    [Header("Visual")]
    [SerializeField]
    private TMP_Text appliedStampText;

    private bool hasAppliedStamp;
    private StampTheme appliedStampTheme;

    public bool HasAppliedStamp =>
        hasAppliedStamp;

    public StampTheme AppliedStampTheme =>
        appliedStampTheme;

    public void ApplyStamp(
        StampTheme stampTheme)
    {
        hasAppliedStamp = true;
        appliedStampTheme = stampTheme;

        UpdateStampVisual();
    }

    public int GetStampScore()
    {
        if (!hasAppliedStamp)
            return 0;

        if (recipientProfile == null)
            return 0;

        return recipientProfile
            .GetStampPreferenceScore(
                appliedStampTheme
            );
    }

    private void UpdateStampVisual()
    {
        if (appliedStampText == null)
            return;

        appliedStampText.text =
            FormatStampName(appliedStampTheme);

        appliedStampText.gameObject.SetActive(true);
    }

    private string FormatStampName(
        StampTheme stampTheme)
    {
        return stampTheme switch
        {
            StampTheme.PopCulture =>
                "POP",

            StampTheme.CozyHome =>
                "HOME",

            StampTheme.Professional =>
                "FORMAL",

            _ =>
                stampTheme.ToString()
                    .ToUpper()
        };
    }
}