namespace Muciojad.SpaceHorror.Gameplay.Traps
{
    using NaughtyAttributes;
    using TrapFacade;
    using TriggerEvents;
    using UnityEngine;
    using Zenject;

    public class TrapTriggerComponent : MonoBehaviour
    {
        #region Public Methods
        public void Activate(bool activate)
        {
            _Active = activate;
            _EventSpot.gameObject.SetActive(_Active);
        }
        #endregion
        
        #region Inspector
        [SerializeField] private TriggerEventSpot _EventSpot;

        [BoxGroup("Trap type")] [SerializeField]
        private TrapType _TrapType;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _TrapFacade.RegisterTrapTrigger(this);
            _EventSpot.EventTriggered += Triggered;
        }
        private void OnDisable()
        {
            _TrapFacade.UnregisterTrapTrigger(this);
            _EventSpot.EventTriggered -= Triggered;
        }
        #endregion

        #region Private Variables
        private bool _Active;
        [Inject] private TrapFacade.TrapFacade _TrapFacade;
        #endregion

        #region Private Methods
        private void Triggered()
        {
            _TrapFacade.RequestTrap(_TrapType);
        }
        #endregion
    }
}