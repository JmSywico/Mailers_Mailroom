using TMPro;
using UnityEngine;

public sealed class MailInspectionController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField]
    private GameObject inspectionPanel;

    [Header("Displayed Information")]
    [SerializeField]
    private TMP_Text mailTypeText;

    [SerializeField]
    private TMP_Text recipientText;

    [SerializeField]
    private TMP_Text addressText;

    [SerializeField]
    private TMP_Text detailsText;

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

        if (mailTypeText != null)
        {
            mailTypeText.text =
                string.IsNullOrWhiteSpace(mailType)
                    ? "MAIL"
                    : mailType;
        }

        if (recipientText != null)
        {
            recipientText.text =
                string.IsNullOrWhiteSpace(recipient)
                    ? "Unknown Recipient"
                    : recipient;
        }

        if (addressText != null)
        {
            addressText.text =
                string.IsNullOrWhiteSpace(address)
                    ? "No address shown"
                    : address;
        }

        if (detailsText != null)
        {
            detailsText.text =
                string.IsNullOrWhiteSpace(details)
                    ? "No additional markings."
                    : details;
        }

        inspectionPanel.SetActive(true);
    }

    public void CloseInspection()
    {
        if (inspectionPanel != null)
            inspectionPanel.SetActive(false);
    }
}