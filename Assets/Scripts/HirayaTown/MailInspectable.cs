using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DraggableMailItem))]
public sealed class MailInspectable :
    MonoBehaviour,
    IPointerClickHandler
{
    [Header("Controller")]
    [SerializeField]
    private MailInspectionController inspectionController;

    [Header("Inspection Information")]
    [SerializeField]
    private string mailType = "LETTER";

    [SerializeField]
    private string recipient;

    [TextArea(2, 4)]
    [SerializeField]
    private string address;

    [TextArea(2, 4)]
    [SerializeField]
    private string details;

    public void OnPointerClick(
        PointerEventData eventData)
    {
        if (eventData.button !=
            PointerEventData.InputButton.Left)
        {
            return;
        }

        if (inspectionController == null)
        {
            Debug.LogError(
                $"{name} has no Inspection Controller assigned.",
                this
            );

            return;
        }

        inspectionController.ShowMail(
            mailType,
            recipient,
            address,
            details
        );
    }
}