namespace Muciojad.SpaceHorror.UserInterface.Diary
{
    using System;
    using System.Collections.Generic;
    using Systems.Notes;
    using Data.Notes;
    using NaughtyAttributes;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class DiaryView : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private DiaryNoteEntry _TemplateEntry;
        [SerializeField] private RectTransform _Container;

        [BoxGroup("Note View")] [SerializeField]
        private TMP_Text _NoteTitle;

        [BoxGroup("Note View")] [SerializeField]
        private TMP_Text _NoteMessage;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            BuildView();
        }
        private void OnDisable()
        {
            foreach (var entry in _Entries)
            {
                entry.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Private Variables
        [Inject] private INotesDiary _Diary;
        private List<DiaryNoteEntry> _Entries = new List<DiaryNoteEntry>();
        #endregion

        #region Private Methods
        private void BuildView()
        {
            //simple reusing existing objects, instatiating new ones only when it's needed
            for (var i = 0; i < _Diary.GetAllNotes().Count; i++)
            {
                var note = _Diary.GetAllNotes()[i];
                if (i >= _Entries.Count)
                {
                    var entry = Instantiate(_TemplateEntry, _Container);
                    entry.Initialize(note.NotePreset, DisplayNote);
                    _Entries.Add(entry);
                    continue;
                }
                _Entries[i].Initialize(note.NotePreset, DisplayNote);
                _Entries[i].gameObject.SetActive(true);
            }

            // disable unused objects
            if (_Entries.Count <= _Diary.GetAllNotes().Count) return;
            {
                for (var i = _Diary.GetAllNotes().Count; i < _Entries.Count; i++)
                {
                    _Entries[i].gameObject.SetActive(false);
                }
            }

            if (_Diary.GetAllNotes().Count <= 0) return;
            // display first note as default
            DisplayNote(_Diary.GetAllNotes()[0].NotePreset);
        }

        private void DisplayNote(NotePreset notePreset)
        {
            _NoteTitle.text = notePreset.Title;
            _NoteMessage.text = notePreset.Message;
        }
        #endregion
    }
}