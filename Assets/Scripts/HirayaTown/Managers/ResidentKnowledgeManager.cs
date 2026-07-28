using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResidentKnowledgeManager : MonoBehaviour
{
    [Header("Resident Notes")]
    [SerializeField]
    private List<ResidentKnowledgeNote> notes =
        new List<ResidentKnowledgeNote>();

    private readonly Dictionary<string, ResidentKnowledgeNote> notesById =
        new Dictionary<string, ResidentKnowledgeNote>(
            StringComparer.OrdinalIgnoreCase
        );

    private readonly HashSet<string> unlockedNoteIds =
        new HashSet<string>(
            StringComparer.OrdinalIgnoreCase
        );

    private bool isInitialized;

    public bool HasKnowledgeNotes =>
        notes.Count > 0;

    public event Action<ResidentKnowledgeNote> NoteUnlocked;

    private void Awake()
    {
        Initialize();
    }

    public bool IsNoteUnlocked(
        string noteId)
    {
        EnsureInitialized();

        string normalizedNoteId =
            NormalizeNoteId(noteId);

        if (string.IsNullOrWhiteSpace(normalizedNoteId))
            return false;

        return unlockedNoteIds.Contains(
            normalizedNoteId
        );
    }

    public bool UnlockNote(
        string noteId)
    {
        EnsureInitialized();

        string normalizedNoteId =
            NormalizeNoteId(noteId);

        if (string.IsNullOrWhiteSpace(normalizedNoteId))
            return false;

        if (!notesById.TryGetValue(
                normalizedNoteId,
                out ResidentKnowledgeNote note))
        {
            Debug.LogWarning(
                $"Resident knowledge note '{noteId}' is not configured.",
                this
            );

            return false;
        }

        bool wasUnlocked =
            unlockedNoteIds.Add(normalizedNoteId);

        if (!wasUnlocked)
            return false;

        NoteUnlocked?.Invoke(note);

        return true;
    }

    public List<ResidentKnowledgeNote> GetUnlockedNotesForResident(
        ResidentProfileData resident)
    {
        EnsureInitialized();

        List<ResidentKnowledgeNote> unlockedNotes =
            new List<ResidentKnowledgeNote>();

        if (resident == null)
            return unlockedNotes;

        foreach (ResidentKnowledgeNote note in notes)
        {
            if (note == null ||
                !note.BelongsToResident(resident))
            {
                continue;
            }

            if (!IsNoteUnlocked(note.NoteId))
                continue;

            string normalizedNoteId =
                NormalizeNoteId(note.NoteId);

            bool isRegisteredNote =
                notesById.TryGetValue(
                    normalizedNoteId,
                    out ResidentKnowledgeNote registeredNote
                ) &&
                registeredNote == note;

            if (!isRegisteredNote)
                continue;

            unlockedNotes.Add(note);
        }

        return unlockedNotes;
    }

    private void Initialize()
    {
        notesById.Clear();
        unlockedNoteIds.Clear();

        foreach (ResidentKnowledgeNote note in notes)
        {
            RegisterNote(note);
        }

        foreach (ResidentKnowledgeNote note in notes)
        {
            if (note == null ||
                !note.BeginsUnlocked)
            {
                continue;
            }

            string normalizedNoteId =
                NormalizeNoteId(note.NoteId);

            if (string.IsNullOrWhiteSpace(normalizedNoteId) ||
                !notesById.ContainsKey(normalizedNoteId))
            {
                continue;
            }

            unlockedNoteIds.Add(normalizedNoteId);
        }

        isInitialized = true;
    }

    private void RegisterNote(
        ResidentKnowledgeNote note)
    {
        if (note == null)
            return;

        string normalizedNoteId =
            NormalizeNoteId(note.NoteId);

        if (string.IsNullOrWhiteSpace(normalizedNoteId))
        {
            Debug.LogWarning(
                "A resident knowledge note is missing a note ID.",
                this
            );

            return;
        }

        if (notesById.ContainsKey(normalizedNoteId))
        {
            Debug.LogWarning(
                $"Duplicate resident knowledge note ID '{note.NoteId}' ignored.",
                this
            );

            return;
        }

        notesById.Add(
            normalizedNoteId,
            note
        );
    }

    private void EnsureInitialized()
    {
        if (isInitialized)
            return;

        Initialize();
    }

    private string NormalizeNoteId(
        string noteId)
    {
        if (noteId == null)
            return string.Empty;

        return noteId.Trim();
    }
}
