using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class MailLevelManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text progressText;

    [SerializeField]
    private TMP_Text stampScoreText;

    [SerializeField]
    private GameObject levelCompletePanel;

    private readonly List<DraggableMailItem> mailItems =
        new List<DraggableMailItem>();

    private int sortedMailCount;
    private int totalStampScore;

    public int SortedMailCount => sortedMailCount;
    public int TotalMailCount => mailItems.Count;
    public int TotalStampScore => totalStampScore;

    public void ConfigureLevel(
        List<DraggableMailItem> newMailItems)
    {
        UnsubscribeFromMailItems();

        mailItems.Clear();

        if (newMailItems != null)
        {
            foreach (DraggableMailItem mailItem in newMailItems)
            {
                if (mailItem == null)
                    continue;

                mailItems.Add(mailItem);
                mailItem.Sorted += HandleMailSorted;
            }
        }

        sortedMailCount = 0;
        totalStampScore = 0;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        UpdateProgressText();
        UpdateStampScoreText();
    }

    private void OnDestroy()
    {
        UnsubscribeFromMailItems();
    }

    private void HandleMailSorted(
        DraggableMailItem sortedMailItem)
    {
        sortedMailCount++;

        StampableMailItem stampableMailItem =
            sortedMailItem.GetComponent<StampableMailItem>();

        if (stampableMailItem != null)
        {
            int stampScore =
                stampableMailItem.GetStampScore();

            totalStampScore += stampScore;

            Debug.Log(
                $"{sortedMailItem.MailItemId} stamp score: " +
                $"{stampScore}. Total: {totalStampScore}.",
                sortedMailItem
            );
        }

        UpdateProgressText();
        UpdateStampScoreText();

        if (sortedMailCount >= mailItems.Count)
            CompleteLevel();
    }

    private void CompleteLevel()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        Debug.Log(
            $"Day completed with stamp score: {totalStampScore}.",
            this
        );
    }

    private void UpdateProgressText()
    {
        if (progressText == null)
            return;

        progressText.text =
            $"{sortedMailCount} / {mailItems.Count} SORTED";
    }

    private void UpdateStampScoreText()
    {
        if (stampScoreText == null)
            return;

        stampScoreText.text =
            $"STAMP SCORE: {totalStampScore}";
    }

    private void UnsubscribeFromMailItems()
    {
        foreach (DraggableMailItem mailItem in mailItems)
        {
            if (mailItem == null)
                continue;

            mailItem.Sorted -= HandleMailSorted;
        }
    }
}