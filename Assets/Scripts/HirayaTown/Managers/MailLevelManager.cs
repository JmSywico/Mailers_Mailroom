using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MailLevelManager : MonoBehaviour
{
    [Header("Level Mail")]
    [SerializeField]
    private List<DraggableMailItem> mailItems =
        new List<DraggableMailItem>();

    [Header("UI")]
    [SerializeField]
    private TMP_Text progressText;

    [SerializeField]
    private GameObject levelCompletePanel;

    private int sortedMailCount;

    private void Start()
    {
        sortedMailCount = 0;

        foreach (DraggableMailItem mailItem in mailItems)
        {
            if (mailItem == null)
                continue;

            mailItem.Sorted += HandleMailSorted;

            if (mailItem.IsSorted)
                sortedMailCount++;
        }

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        UpdateProgressText();
    }

    private void OnDestroy()
    {
        foreach (DraggableMailItem mailItem in mailItems)
        {
            if (mailItem == null)
                continue;

            mailItem.Sorted -= HandleMailSorted;
        }
    }

    private void HandleMailSorted(
        DraggableMailItem sortedMailItem)
    {
        sortedMailCount++;

        UpdateProgressText();

        if (sortedMailCount >= mailItems.Count)
            CompleteLevel();
    }

    private void UpdateProgressText()
    {
        if (progressText == null)
            return;

        progressText.text =
            $"{sortedMailCount} / {mailItems.Count} SORTED";
    }

    private void CompleteLevel()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        Debug.Log("Level completed.", this);
    }

    public void RestartLevel()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}