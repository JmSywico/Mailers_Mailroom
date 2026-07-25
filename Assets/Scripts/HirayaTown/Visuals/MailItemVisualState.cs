using UnityEngine;

public sealed class MailItemVisualState : MonoBehaviour
{
    [Header("Hide When Delivered")]
    [SerializeField]
    private GameObject[] deskOnlyObjects;

    [Header("Show When Delivered")]
    [SerializeField]
    private GameObject deliveredVisual;

    public void ApplyDeliveredState()
    {
        foreach (GameObject deskObject in deskOnlyObjects)
        {
            if (deskObject != null)
                deskObject.SetActive(false);
        }

        if (deliveredVisual != null)
            deliveredVisual.SetActive(true);
    }
}