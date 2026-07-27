using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class DayManager : MonoBehaviour
{
    [Serializable]
    private sealed class DayContent
    {
        public string dayName;

        public List<GameObject> activeObjects =
            new List<GameObject>();

        public List<DraggableMailItem> mailItems =
            new List<DraggableMailItem>();
    }

    [Header("References")]
    [SerializeField]
    private MailLevelManager levelManager;

    [SerializeField]
    private StampSelectionController stampSelectionController;

    [SerializeField]
    private TMP_Text dayText;

    [Header("Days")]
    [SerializeField]
    private List<DayContent> days =
        new List<DayContent>();

    [SerializeField]
    private int startingDayIndex;

    private int currentDayIndex;

    public int CurrentDayIndex =>
        currentDayIndex;

    public bool HasNextDay =>
        currentDayIndex < days.Count - 1;

    private void Start()
    {
        LoadDay(startingDayIndex);
    }

    public void LoadDay(int dayIndex)
    {
        if (dayIndex < 0 ||
            dayIndex >= days.Count)
        {
            Debug.LogError(
                $"Day index {dayIndex} is invalid.",
                this
            );

            return;
        }

        DisableAllDayObjects();

        currentDayIndex = dayIndex;

        DayContent currentDay =
            days[currentDayIndex];

        foreach (GameObject dayObject
                 in currentDay.activeObjects)
        {
            if (dayObject != null)
                dayObject.SetActive(true);
        }

        if (dayText != null)
        {
            dayText.text =
                string.IsNullOrWhiteSpace(
                    currentDay.dayName)
                    ? $"DAY {currentDayIndex + 1}"
                    : currentDay.dayName;
        }

        if (stampSelectionController != null)
            stampSelectionController.ClearSelection();

        if (levelManager != null)
        {
            levelManager.ConfigureLevel(
                currentDay.mailItems
            );
        }
    }

    public void LoadNextDay()
    {
        if (!HasNextDay)
        {
            Debug.Log(
                "There are no more configured days.",
                this
            );

            return;
        }

        LoadDay(currentDayIndex + 1);
    }

    private void DisableAllDayObjects()
    {
        foreach (DayContent day in days)
        {
            foreach (GameObject dayObject
                     in day.activeObjects)
            {
                if (dayObject != null)
                    dayObject.SetActive(false);
            }
        }
    }
}