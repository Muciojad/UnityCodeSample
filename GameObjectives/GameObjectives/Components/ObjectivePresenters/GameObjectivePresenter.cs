namespace Muciojad.SpaceHorror.Systems.GameObjectives.Components
{
    using Data.Objectives;
    using Notifications;
    using Signals.UserInterface;
    using UnityEngine;
    using UserInterface.Panels;
    using Zenject;

    public class GameObjectivePresenter : MonoBehaviour
    {
        #region Inspector
        [SerializeField] protected GameObjective _HandledObjective;
        [SerializeField] protected NotificationPreset _FinishedObjectiveNotification;
        #endregion
        #region Unity Methods
        protected void Awake()
        {
            _ObjectiveInformer = _HandledObjective;
        }
        #endregion
        #region Private Variables
        protected IGameObjectiveInformer _ObjectiveInformer;
        [Inject] protected IGameObjectivesAmountChangedReceiver _AmountChangedReceiver;
        [Inject] protected SignalBus _SignalBus;
        protected int _CurrentObjectiveAmount;
        #endregion
        #region Private Methods
        protected virtual void HandleProgress()
        {
            _ObjectiveInformer.OnProgressed(_CurrentObjectiveAmount);
            _AmountChangedReceiver.NotifyObjectiveCollectedAmountChanged(_HandledObjective,_CurrentObjectiveAmount);
            // fill with proper logic in derived classes
        }
        protected virtual void HandleTaskFinished()
        {
            _ObjectiveInformer.Finished();
            NotifyFinished();
            // fill with proper logic in derived classes
        }
        
        protected void NotifyFinished()
        {
            var signal = new ShowNotificationPanelSignal(_FinishedObjectiveNotification.Message, _FinishedObjectiveNotification.NotificationType);
            signal.SetPanelProperties(PanelType.NotificationPanel,false);
            _SignalBus.Fire(signal);
        }
        #endregion
    }
}