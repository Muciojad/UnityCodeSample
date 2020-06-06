namespace Muciojad.SpaceHorror.Gameplay.Traps.TrapController.Traps
{
    using System.Collections.Generic;
    using DG.Tweening;
    using MEC;
    using NaughtyAttributes;
    using Player;
    using PlayerHand;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Choke trap component.
    /// By design traps can fetch all data/components needed to proper working.
    /// </summary>
    public class ChokeTrap : TrapController
    {
        #region Inspector
        [BoxGroup("References")] [SerializeField] private Transform _PlayerHangPosition;

        [BoxGroup("References")] [SerializeField] private TrapTriggerComponent _TrapTriggerComponent;

        [BoxGroup("Times")] [SerializeField] private float _RestoreValuesTime;

        [SerializeField] [BoxGroup("Minigame")]
        private ChokeEscapeMinigame _Minigame;
        #endregion


        #region Unity Methods
        protected override void OnEnable()
        {
            base.OnEnable();
            _Minigame.MinigameCompleted += OnMinigameCompleted;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _Minigame.MinigameCompleted -= OnMinigameCompleted;
        }
        #endregion

        #region Private Variables
        private Vector3 _PlayerPositionRestore;
        private Vector3 _PlayerRotationRestore = new Vector3(0f,90f,0f);
        private const float _SmoothTransitionsDuration = 1f;
        [Inject] private Player _Player;
        #endregion
        
        #region Private Methods
        protected override void Activate()
        {
            // set up all values needed to start trap minigame
            _PlayerPositionRestore = _Player.Position;
            _Player.MainTransform.DOMove(_PlayerHangPosition.position, _SmoothTransitionsDuration);
            _Player.MainTransform.DORotate(_PlayerHangPosition.eulerAngles, _SmoothTransitionsDuration);

            var playerRB = _Player.MainTransform.GetComponent<Rigidbody>();
            if (playerRB != null)
            {
                playerRB.isKinematic = true;
            }
            _Minigame.Activate(_Player.MainTransform);
        }

        protected override void Deactivate()
        {
            // restore all values that changed during minigame
            _Player.MainTransform.DOMove(_PlayerPositionRestore, _RestoreValuesTime);
            _Player.MainTransform.DORotate(_PlayerRotationRestore, _RestoreValuesTime);
            var playerRB = _Player.MainTransform.GetComponent<Rigidbody>();
            if (playerRB != null)
            {
                playerRB.isKinematic = false;
            }
            _TrapTriggerComponent.gameObject.SetActive(false);
            base.Deactivate();

        }
        
        private void OnMinigameCompleted(bool success)
        {
            if (success)
            {
                Deactivate();
            }
            else
            {
                Debug.LogError("You just died in choke trap.");
                _Player.Damagable.TakeDamage(_Player.Health.CurrentHealth);
            }
        }

        #endregion
    }
}