namespace Muciojad.SpaceHorror.Gameplay.Traps.TrapController
{
    using System;
    using NaughtyAttributes;
    using TrapFacade;
    using UnityEngine;
    using Zenject;

    public class TrapController : MonoBehaviour, ITrap
    {
        #region Public Variables
        TrapType ITrap.TrapType => _TrapType;
        #endregion
    
        #region Public Methods
        void ITrap.Activate()
        {
            Debug.Log($"Activated trap {_TrapType}");
            _Activated?.Invoke();
            Activate();
        }
        void ITrap.OnDeactivate()
        {
            Debug.Log($"Deactivated trap {_TrapType}");
            _Deactivated?.Invoke();
        }
        event Action ITrap.Activated
        {
            add => _Activated += value;
            remove => _Activated -= value;
        }

        event Action ITrap.Deactivated
        {
            add => _Deactivated += value;
            remove => _Deactivated -= value;
        }
        #endregion
        
        #region Inspector
        [BoxGroup("TrapType")] [SerializeField]
        protected TrapType _TrapType;
        #endregion

        #region Unity Methods
        protected virtual void OnEnable()
        {
            _TrapFacade.RegisterTrap(this);
        }
        protected virtual void OnDisable()
        {
            _TrapFacade.UnregisterTrap(this);
        }
        #endregion
        #region Private Variables
        [Inject] protected ITrapFacade _TrapFacade;
        protected event Action _Activated;
        protected event Action _Deactivated;
        #endregion

        #region Private Methods
        protected virtual void Activate()
        {
            // fill with logic required to properly setup minigame
        }

        protected virtual void Deactivate()
        {
            ((ITrap) this).OnDeactivate();
            // fill with logic required to properly unload minigame and restore in example player position etc
        }
        #endregion



    }
}