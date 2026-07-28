using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableMailItem))]
public sealed class MailKnowledgeReward : MonoBehaviour
{
    [SerializeField]
    private ResidentKnowledgeManager knowledgeManager;

    [SerializeField]
    private List<string> noteIdsToUnlock =
        new List<string>();

    private DraggableMailItem mailItem;

    private void Awake()
    {
        mailItem =
            GetComponent<DraggableMailItem>();
    }

    private void OnEnable()
    {
        if (mailItem == null)
        {
            mailItem =
                GetComponent<DraggableMailItem>();
        }

        if (mailItem != null)
            mailItem.Sorted += HandleMailSorted;
    }

    private void OnDisable()
    {
        if (mailItem != null)
            mailItem.Sorted -= HandleMailSorted;
    }

    private void HandleMailSorted(
        DraggableMailItem sortedMailItem)
    {
        if (noteIdsToUnlock.Count == 0)
            return;

        ResidentKnowledgeManager activeKnowledgeManager =
            ResolveKnowledgeManager();

        if (activeKnowledgeManager == null)
        {
            Debug.LogError(
                $"{name} has knowledge rewards but no ResidentKnowledgeManager was found.",
                this
            );

            return;
        }

        foreach (string noteId in noteIdsToUnlock)
        {
            activeKnowledgeManager.UnlockNote(
                noteId
            );
        }
    }

    private ResidentKnowledgeManager ResolveKnowledgeManager()
    {
        if (knowledgeManager != null)
            return knowledgeManager;

        knowledgeManager =
            UnityEngine.Object
                .FindFirstObjectByType<ResidentKnowledgeManager>();

        return knowledgeManager;
    }
}
