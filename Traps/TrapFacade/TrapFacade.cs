namespace Muciojad.SpaceHorror.Gameplay.Traps.TrapFacade
{
    using System.Collections.Generic;
    using System.Linq;
    using Systems.Input;
    using Zenject;

    public class TrapFacade : IInitializable, ITrapRequestHandler, ITrapFacade, ITrapTriggerHolder
    {
        #region Public Methods
        void ITrapFacade.RegisterTrap(ITrap trap)
        {
            _Traps.Add(trap);
            trap.Activated += HandleTrapActivation;
            trap.Deactivated += HandleTrapDeactivation;
        }
        void ITrapFacade.UnregisterTrap(ITrap trap)
        {
            trap.Activated -= HandleTrapActivation;
            trap.Deactivated -= HandleTrapDeactivation;
            _Traps.Remove(trap);
        }

        void ITrapRequestHandler.RequestTrap(TrapType trapType)
        {
            var matchingTrap = _Traps.FirstOrDefault(t => t.TrapType.Equals(trapType));
            matchingTrap?.Activate();
        }

        void ITrapTriggerHolder.RegisterTrapTrigger(TrapTriggerComponent triggerComponent)
        {
            _TrapTriggers.Add(triggerComponent);
        }
        void ITrapTriggerHolder.UnregisterTrapTrigger(TrapTriggerComponent triggerComponent)
        {
            _TrapTriggers.Remove(triggerComponent);
        }


        public void EnableTraps(bool enable)
        {
            foreach (var trapTrigger in _TrapTriggers)
            {
                trapTrigger.Activate(enable);
            }
        }

        void IInitializable.Initialize()
        {
            EnableTraps(false);
        }
        
        #endregion
        
        #region Private Variables
        private List<ITrap> _Traps = new List<ITrap>();
        private List<TrapTriggerComponent> _TrapTriggers = new List<TrapTriggerComponent>();
        [Inject] private GameInput _GameInput;
        #endregion
        #region Private Methods
        private void HandleTrapActivation()
        {
            _GameInput.SetMinigameMap();
            // change input category
        }
        private void HandleTrapDeactivation()
        {
            _GameInput.SetGameplayMap();
            // restore input category
        }
        #endregion
    }
}