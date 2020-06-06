namespace Muciojad.SpaceHorror.Gameplay.Traps.TrapController.Traps
{
    using System;
    using System.Collections.Generic;
    using Systems.Input;
    using _Scripts.Signals.UserInterface;
    using Damage.Components;
    using DG.Tweening;
    using MEC;
    using NaughtyAttributes;
    using UnityEngine;
    using UserInterface.Panels;
    using Zenject;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Minigame component.
    /// All minigames should be implemented as self-sufficient (for example should handle input needed to perform minigams actions).
    /// </summary>
    public class ChokeEscapeMinigame : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Passing normalized value of minigame progress.
        /// </summary>
        public event Action<float> MinigameProgress;
        public event Action<bool> MinigameCompleted;
        #endregion
        #region Public Methods
        public void Activate(Transform hangedTransform)
        {
            _HangedTransform = hangedTransform;
            _DamageZoneTrigger.Switch(false);
            _TimeoutHandle = Timing.RunCoroutine(MinigameCounter().CancelWith(gameObject));
            _SwingHandle = Timing.RunCoroutine(SwingCO().CancelWith(gameObject));
            ShowUI(true);
        }
        #endregion
        #region Inspector
        [SerializeField] [BoxGroup("Settings")]
        private float _MinigameTime;

        [SerializeField] [BoxGroup("Settings")]
        private DamageZoneTrigger _DamageZoneTrigger;

        [SerializeField] [BoxGroup("Settings")]
        private Vector2 _SwingMinMax;

        [SerializeField] [BoxGroup("Settings")]
        private float _SwingAngleChangeTime;

        [SerializeField] [BoxGroup("Settings")]
        private float _InputCooldown;

        [SerializeField] [BoxGroup("Settings")]
        private float _SingleProgressStep;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            // switch damage zone to hurt player during minigame
            _DamageZoneTrigger.Switch(true);
            _GameInput.OnMinigameQActionDown += HandleInput;
        }
        private void OnDisable()
        {
            _GameInput.OnMinigameQActionDown -= HandleInput;
        }
        #endregion

        #region Private Variables
        private float _Progress;
        private CoroutineHandle _TimeoutHandle;
        private CoroutineHandle _SwingHandle;
        private CoroutineHandle _InputCooldownHandle;
        private bool _InputLocked;
        private Transform _HangedTransform;
        [Inject] private SignalBus _SignalBus;
        [Inject] private GameInput _GameInput;
        #endregion
        
        #region Private Methods

        private void ProgressMinigame(float progress)
        {
            _Progress += progress;
            if (_Progress >= 1f)
            {
                _Progress = 1f;
                Success();
                return;
            }
            MinigameProgress?.Invoke(_Progress);
        }

        private IEnumerator<float> SwingCO()
        {
            while (_Progress < 1f)
            {
                var targetAngle = Random.Range(_SwingMinMax.x, _SwingMinMax.y);
                _HangedTransform.DORotate(
                    new Vector3(_HangedTransform.rotation.x,_HangedTransform.rotation.y , targetAngle), _SwingAngleChangeTime);
                yield return Timing.WaitForSeconds(_SwingAngleChangeTime);
            }
        }
        
        private IEnumerator<float> MinigameCounter()
        {
            yield return Timing.WaitForSeconds(_MinigameTime);
            Failure();
        }
        
        private IEnumerator<float> InputCooldownCO()
        {
            _InputLocked = true;
            yield return Timing.WaitForSeconds(_InputCooldown);
            _InputLocked = false;
        }

        private void Success()
        {
            _DamageZoneTrigger.Switch(false);
            MinigameCompleted?.Invoke(true);
            Timing.KillCoroutines(_TimeoutHandle);
            Timing.KillCoroutines(_SwingHandle);
            ShowUI(false);
        }

        private void Failure()
        {
            MinigameCompleted?.Invoke(false);

            Timing.KillCoroutines(_TimeoutHandle);
            Timing.KillCoroutines(_SwingHandle);
            ShowUI(false);
        }

        private void ShowUI(bool show)
        {
            var signal = new ShowChokeMinigamePanelSignal();
            signal.SetPanelProperties(PanelType.ChokeMinigamePanel,!show);
            _SignalBus.Fire(signal);
        }

        private void HandleInput()
        {
            if (_InputLocked) return;
            ProgressMinigame(_SingleProgressStep);
            _InputLocked = true;
            _InputCooldownHandle = Timing.RunCoroutine(InputCooldownCO().CancelWith(gameObject));
        }
        
        #endregion
    }
}