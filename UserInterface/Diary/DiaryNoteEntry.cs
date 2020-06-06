namespace Muciojad.SpaceHorror.UserInterface.Diary
{
    using System;
    using Data.Notes;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DiaryNoteEntry : MonoBehaviour
    {
        #region Public Methods
        public void Initialize(NotePreset preset, Action<NotePreset> clickAction = null)
        {
            _ClickAction = null;
            _Icon.sprite = preset.NoteIcon;
            _MessageLabel.text = preset.Title;
            _StoredPreset = preset;
            _ClickAction += clickAction;
        }
        #endregion
        
        #region Inspector
        [SerializeField] private Image _Icon;
        [SerializeField] private TMP_Text _MessageLabel;
        [SerializeField] private Button _Button;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _Button.onClick.AddListener(Clicked);
        }
        private void OnDisable()
        {
            _Button.onClick.RemoveListener(Clicked);
        }
        #endregion

        #region Private Variables
        private Action<NotePreset> _ClickAction;
        private NotePreset _StoredPreset;
        #endregion
        #region Private Methods
        private void Clicked()
        {
            _ClickAction?.Invoke(_StoredPreset);
        }
        #endregion
    }
}