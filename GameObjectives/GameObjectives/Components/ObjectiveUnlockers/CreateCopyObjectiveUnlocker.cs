namespace Muciojad.SpaceHorror.Systems.GameObjectives.Components.ObjectiveUnlocker
{
    using Gameplay.Console.Extensions.ViewEventHandler;
    using Zenject;

    public class CreateCopyObjectiveUnlocker : GameObjectiveUnlockerComponent
    {
        #region Unity Methods
        private void OnEnable()
        {
            _ConsoleViewEventHandler.DeepCopyViewOpened += CopyViewOpened;
        }
        private void OnDisable()
        {
            _ConsoleViewEventHandler.DeepCopyViewOpened -= CopyViewOpened;
        }
        #endregion
        #region Private Variables
        [Inject] private ConsoleViewEventHandler _ConsoleViewEventHandler;
        #endregion
        #region Private Methods
        private void CopyViewOpened()
        {
            Unlock();
            gameObject.SetActive(false);
        }
        #endregion
    }
}