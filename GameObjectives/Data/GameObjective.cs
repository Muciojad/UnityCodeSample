namespace Muciojad.SpaceHorror.Data.Objectives
{
    using System;
    using NaughtyAttributes;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Muciojad/GameObjectives/Objective")]
    public class GameObjective : ScriptableObject, IGameObjectiveInformer, IGameObjectiveUnlocker
    {
        #region Public Types
         public enum GameObjectiveType
         {
            Do,
            Collect,
            Reach
         }
         #endregion
            
        #region Public Variables
        /// <summary>
        /// Special event to inform subjects about task progress.
        /// Intended to be used by UI components.
        /// </summary>
        public event Action<int> Progressed;
        
        /// <summary>
        /// Special event to inform subjects that task is finished.
        /// Intended to be used by UI components.
        /// </summary>
        public event Action Finished;

        public string ObjectiveName => _ObjectiveName;
        public string ObjectiveDescription => _ObjectiveShortDescription;
        public GameObjectiveType ObjectiveType => _ObjectiveType;
        public int RequiredAmount => _RequiredAmount;
        public bool Unlocked => _Unlocked;
        public bool IsFinished => _IsFinished;
        #endregion

        #region Public Methods
        void IGameObjectiveInformer.OnProgressed(int amount)
        {
                Progressed?.Invoke(amount);
        }
        void IGameObjectiveInformer.Finished()
        {
#if !UNITY_EDITOR
                _IsFinished = true;
#endif
#if UNITY_EDITOR
                Debug.Log($"Finished {ObjectiveName}");
#endif
                Finished?.Invoke();
        }

        void IGameObjectiveUnlocker.Unlock()
        {
#if UNITY_EDITOR
                Debug.Log($"Unlocked {ObjectiveName}");
#endif
#if !UNITY_EDITOR
                _Unlocked = true;
#endif
        }
        #endregion
        
        #region Inspector
        [BoxGroup("Objective display data")] [SerializeField]
        private string _ObjectiveName;

        [BoxGroup("Objective display data")] [SerializeField] [Multiline(5)]
        private string _ObjectiveShortDescription;

        [BoxGroup("Objective Type")] [SerializeField]
        private GameObjectiveType _ObjectiveType;

        [BoxGroup("Objective Type")] [SerializeField]
#if UNITY_EDITOR
        [ShowIf(nameof(_ShowAmountField))]
#endif
        private int _RequiredAmount;

        [BoxGroup("Objective Unlock State")] [SerializeField]
        private bool _Unlocked;

        [BoxGroup("Preview")] [SerializeField] private bool _IsFinished;
        #endregion

        #region Editor Variables
#if UNITY_EDITOR
        private bool _ShowAmountField;
#endif
        #endregion

        #region Unity Methods
#if UNITY_EDITOR
            private void OnValidate()
            {
                    _ShowAmountField = _ObjectiveType == GameObjectiveType.Collect;
            }
#endif
        #endregion
    }


    
}