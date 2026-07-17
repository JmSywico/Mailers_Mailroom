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

    public string ResidentId => residentId;
    public string DisplayName => displayName;
    public Sprite Portrait => portrait;
    public string Summary => summary;
    public string Address => address;
    public string Aliases => aliases;
    public string Likes => likes;
    public string Dislikes => dislikes;
    public string DeliveryNotes => deliveryNotes;
}