namespace Muciojad.SpaceHorror.Systems.GameObjectives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Objectives;
    using UnityEditor;
    using UnityEngine;
    using Zenject;

    public class GameObjectivesFacade : IGameObjectivesAmountChangedReceiver, IInitializable
    {
        #region Public Types
        [Serializable]
        public class Settings
        {
            #region Public Variables
            public List<GameObjective> Objectives => _Objectives;
            #endregion
            #region Inspector
            [SerializeField] private List<GameObjective> _Objectives;
            #endregion

            #region Private Methods

            public void ResolveObjectives()
            {
#if UNITY_EDITOR
                var guids = AssetDatabase.FindAssets("t:GameObjective");
                var objectives = new List<GameObjective>();
                foreach (var guid in guids)
                {
                    var objAsset = AssetDatabase.GUIDToAssetPath(guid);
                    objectives.Add(AssetDatabase.LoadAssetAtPath<GameObjective>(objAsset));
                }

                _Objectives = objectives;
#endif
            }
            
            #endregion
        }
        #endregion

        #region Public Variables
        public List<GameObjective> Objectives => _Settings.Objectives;
        #endregion

        #region Public Methods
        public int GetCurrentCollectedAmountForObjective(GameObjective objective)
        {
            if (_ObjectiveCurrentAmountMap.ContainsKey(objective))
            {
                return _ObjectiveCurrentAmountMap[objective];
            }
            return -1;
        }
        
        void IGameObjectivesAmountChangedReceiver.NotifyObjectiveCollectedAmountChanged(GameObjective objective, int newAmount)
        {
            if (_ObjectiveCurrentAmountMap.ContainsKey(objective))
            {
                _ObjectiveCurrentAmountMap[objective] = newAmount;
            }
            else Debug.LogError($"{objective} not present in dictionary!");
        }

        void IInitializable.Initialize()
        {
            GetCollectableObjectives();
        }
        #endregion

        #region Private Variables
        [Inject] private Settings _Settings;
        private Dictionary<GameObjective, int> _ObjectiveCurrentAmountMap = new Dictionary<GameObjective, int>();
        #endregion

        #region Private Methods
        private void GetCollectableObjectives()
        {
            foreach (var gameObjective in Objectives.Where(gameObjective => gameObjective.ObjectiveType == GameObjectiveType.Collect))
            {
                _ObjectiveCurrentAmountMap.Add(gameObjective,0);
            }
        }
        #endregion
    }
}