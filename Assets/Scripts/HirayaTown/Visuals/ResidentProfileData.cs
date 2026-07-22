using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ResidentProfile_",
    menuName = "Hiraya Town/Resident Profile"
)]
public sealed class ResidentProfileData : ScriptableObject
{
    [Header("Identity")]
    [SerializeField]
    private string residentId;

    [SerializeField]
    private string displayName;

    [SerializeField]
    private Sprite portrait;

    [Header("Profile")]
    [TextArea(2, 4)]
    [SerializeField]
    private string summary;

    [TextArea(2, 4)]
    [SerializeField]
    private string address;

    [TextArea(2, 5)]
    [SerializeField]
    private string aliases;

    [TextArea(2, 5)]
    [SerializeField]
    private string likes;

    [TextArea(2, 5)]
    [SerializeField]
    private string dislikes;

    [TextArea(3, 6)]
    [SerializeField]
    private string deliveryNotes;

    [Header("Stamp Preferences")]
    [SerializeField]
    private List<StampTheme> likedStampThemes =
        new List<StampTheme>();

    [SerializeField]
    private List<StampTheme> dislikedStampThemes =
        new List<StampTheme>();

    public string ResidentId => residentId;
    public string DisplayName => displayName;
    public Sprite Portrait => portrait;
    public string Summary => summary;
    public string Address => address;
    public string Aliases => aliases;
    public string Likes => likes;
    public string Dislikes => dislikes;
    public string DeliveryNotes => deliveryNotes;

    public int GetStampPreferenceScore(
        StampTheme stampTheme)
    {
        if (likedStampThemes.Contains(stampTheme))
            return 1;

        if (dislikedStampThemes.Contains(stampTheme))
            return -1;

        return 0;
    }
}