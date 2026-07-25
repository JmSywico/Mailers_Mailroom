using System;
using TMPro;
using UnityEngine;

public sealed class MailInspectionController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField]
    private GameObject inspectionPanel;

    [SerializeField]
    private TMP_Text inspectionTitleText;

    [Header("Letter Preview")]
    [SerializeField]
    private GameObject letterPreview;

    [SerializeField]
    private TMP_Text letterMailTypeText;

    [SerializeField]
    private TMP_Text letterRecipientText;

    [SerializeField]
    private TMP_Text letterAddressText;

    [SerializeField]
    private TMP_Text letterDetailsText;

    [Header("Package Preview")]
    [SerializeField]
    private GameObject packagePreview;

    [SerializeField]
    private TMP_Text packageRecipientText;

    [SerializeField]
    private TMP_Text packageAddressText;

    [SerializeField]
    private TMP_Text packageDetailsText;

    public bool IsOpen =>
        inspectionPanel != null &&
        inspectionPanel.activeSelf;

    private void Start()
    {
        CloseInspection();
    }

    public void ShowMail(
        string mailType,
        string recipient,
        string address,
        string details)
    {
        if (inspectionPanel == null)
        {
            Debug.LogError(
                "The Mail Inspection Panel is not assigned.",
                this
            );

            return;
        }

        bool isPackage =
            string.Equals(
                mailType?.Trim(),
                "PACKAGE",
                StringComparison.OrdinalIgnoreCase
            );

        if (letterPreview != null)
            letterPreview.SetActive(!isPackage);

        if (packagePreview != null)
            packagePreview.SetActive(isPackage);

        if (inspectionTitleText != null)
        {
            inspectionTitleText.text =
                isPackage
                    ? "INSPECT PACKAGE"
                    : "INSPECT LETTER";
        }

        if (isPackage)
        {
            ShowPackage(
                recipient,
                address,
                details
            );
        }
        else
        {
            ShowLetter(
                mailType,
                recipient,
                address,
                details
            );
        }

        inspectionPanel.SetActive(true);
    }

    public void CloseInspection()
    {
        if (inspectionPanel != null)
            inspectionPanel.SetActive(false);
    }

    private void ShowLetter(
        string mailType,
        string recipient,
        string address,
        string details)
    {
        if (letterMailTypeText != null)
        {
            letterMailTypeText.text =
                string.IsNullOrWhiteSpace(mailType)
                    ? "LETTER"
                    : mailType;
        }

        if (letterRecipientText != null)
        {
            letterRecipientText.text =
                string.IsNullOrWhiteSpace(recipient)
                    ? "Unknown Recipient"
                    : recipient;
        }

        if (letterAddressText != null)
        {
            letterAddressText.text =
                string.IsNullOrWhiteSpace(address)
                    ? "No address shown"
                    : address;
        }

        if (letterDetailsText != null)
        {
            letterDetailsText.text =
                string.IsNullOrWhiteSpace(details)
                    ? "No additional markings."
                    : details;
        }
    }

    private void ShowPackage(
        string recipient,
        string address,
        string details)
    {
        if (packageRecipientText != null)
        {
            packageRecipientText.text =
                string.IsNullOrWhiteSpace(recipient)
                    ? "Unknown Recipient"
                    : $"TO:\n{recipient}";
        }

        if (packageAddressText != null)
        {
            packageAddressText.text =
                string.IsNullOrWhiteSpace(address)
                    ? "No address shown"
                    : address;
        }

        if (packageDetailsText != null)
        {
            packageDetailsText.text =
                string.IsNullOrWhiteSpace(details)
                    ? "No visible package markings."
                    : details;
        }
    }
}