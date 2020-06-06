namespace Muciojad.SpaceHorror.Systems.GameObjectives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Objectives;
    using NaughtyAttributes;
    using UnityEngine;
    using Zenject;

    public class GameObjectivesWatcher : IInitializable, IDisposable
    {
        #region Public Types
        [Serializable]
        public sealed class Settings
        {
            #region Public Variables
            public List<GameObjective> ImportantObjectives => _ImportantObjectives;
            #endregion
            
            #region Inspector
            [ReorderableList] [SerializeField] private List<GameObjective> _ImportantObjectives;
            #endregion
        }
        #endregion

        #region Public Variables
        public event Action ObjectivesCompleted;
        #endregion
        
        #region Public Methods
        void IInitializable.Initialize()
        {
            SubscribeToEvents();
        }
        void IDisposable.Dispose()
        {
            Unsubscribe();
        }
        #endregion
        #region Private Variables
        [Inject] private Settings _Settings;
        #endregion
        #region Private Methods
        private void SubscribeToEvents()
        {
            foreach (var objective in _Settings.ImportantObjectives)
            {
                objective.Finished += IsGameFinished;
            }
        }
        private void Unsubscribe()
        {
            foreach (var objective in _Settings.ImportantObjectives)
            {
                objective.Finished -= IsGameFinished;
            }
        }

        private void IsGameFinished()
        {
            if (CheckIfAllFinished())
            {
                ObjectivesCompleted?.Invoke();
            }
        }
        
        private bool CheckIfAllFinished()
        {
            var notCompleted = _Settings.ImportantObjectives.FirstOrDefault(o => !o.IsFinished);
            return notCompleted == null;
        }
        #endregion
    }
}