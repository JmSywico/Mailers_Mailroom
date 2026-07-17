using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PostalHandbookController : MonoBehaviour
{
    [Serializable]
    private sealed class HandbookPage
    {
        public string title;

        [TextArea(5, 12)]
        public string body;
    }

    [Header("Panel")]
    [SerializeField]
    private GameObject handbookPanel;

    [Header("Page Display")]
    [SerializeField]
    private TMP_Text pageTitleText;

    [SerializeField]
    private TMP_Text pageBodyText;

    [SerializeField]
    private TMP_Text pageNumberText;

    [Header("Navigation")]
    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private Button nextButton;

    [Header("Content")]
    [SerializeField]
    private List<HandbookPage> pages =
        new List<HandbookPage>();

    private int currentPageIndex;

    private void Awake()
    {
        if (handbookPanel != null)
            handbookPanel.SetActive(false);
    }

    public void OpenHandbook()
    {
        if (handbookPanel == null)
        {
            Debug.LogError(
                "The Handbook Panel is not assigned.",
                this
            );

            return;
        }

        if (pages.Count == 0)
        {
            Debug.LogError(
                "The handbook does not contain any pages.",
                this
            );

            return;
        }

        currentPageIndex = 0;
        handbookPanel.SetActive(true);

        RefreshPage();
    }

    public void CloseHandbook()
    {
        if (handbookPanel != null)
            handbookPanel.SetActive(false);
    }

    public void ShowPreviousPage()
    {
        if (currentPageIndex <= 0)
            return;

        currentPageIndex--;

        RefreshPage();
    }

    public void ShowNextPage()
    {
        if (currentPageIndex >= pages.Count - 1)
            return;

        currentPageIndex++;

        RefreshPage();
    }

    private void RefreshPage()
    {
        if (pages.Count == 0)
            return;

        HandbookPage currentPage =
            pages[currentPageIndex];

        if (pageTitleText != null)
            pageTitleText.text = currentPage.title;

        if (pageBodyText != null)
            pageBodyText.text = currentPage.body;

        if (pageNumberText != null)
        {
            pageNumberText.text =
                $"{currentPageIndex + 1} / {pages.Count}";
        }

        if (previousButton != null)
        {
            previousButton.interactable =
                currentPageIndex > 0;
        }

        if (nextButton != null)
        {
            nextButton.interactable =
                currentPageIndex < pages.Count - 1;
        }
    }
}