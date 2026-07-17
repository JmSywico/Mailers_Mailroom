using System.Collections.Generic;
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

    [Header("Resident Data")]
    [SerializeField]
    private List<ResidentProfileData> residents =
        new List<ResidentProfileData>();

    private void Awake()
    {
        if (directoryPanel != null)
            directoryPanel.SetActive(false);
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
}