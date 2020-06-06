namespace Muciojad.SpaceHorror.Systems.GameObjectives.Components.ObjectiveUnlocker
{
    using Data.Objectives;
    using Notifications;
    using Signals.UserInterface;
    using UnityEngine;
    using UserInterface.Panels;
    using Zenject;

    public class GameObjectiveUnlockerComponent : MonoBehaviour
    {
        #region Inspector
        [SerializeField] protected GameObjective _Objective;
        [SerializeField] protected NotificationPreset _ObjectivesUpdatedNotification;
        #endregion
        #region Unity Methods
        private void Awake()
        {
            _Unlocker = _Objective;
        }
        #endregion
        #region Private Variables
        protected IGameObjectiveUnlocker _Unlocker;
        [Inject] private SignalBus _SignalBus;
        #endregion
        #region Private Methods
        protected virtual void Unlock()
        {
            _Unlocker.Unlock();
            NotifyUnlocked();
        }
        
        private void NotifyUnlocked()
        {
            var signal = new ShowNotificationPanelSignal(_ObjectivesUpdatedNotification.Message, _ObjectivesUpdatedNotification.NotificationType);
            signal.SetPanelProperties(PanelType.NotificationPanel,false);
            _SignalBus.Fire(signal);
        }
        #endregion
    }
}