namespace Muciojad.SpaceHorror.UserInterface.Diary
{
    using Input;

    public class DiaryScreenInput : ScreenInput
    {
        #region Unity Methods
        private void OnEnable()
        {
            _GameInput.OnDiaryButtonDown += ShowScreen;
        }
        private void OnDisable()
        {
            _GameInput.OnDiaryButtonDown -= ShowScreen;
        }
        #endregion
    }
}