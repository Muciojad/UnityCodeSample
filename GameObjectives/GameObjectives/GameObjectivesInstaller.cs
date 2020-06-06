namespace Muciojad.SpaceHorror.Systems.GameObjectives
{
    using NaughtyAttributes;
    using UnityEngine;
    using Zenject;

    public class GameObjectivesInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Settings).AsSingle().NonLazy();
            Container.BindInstance(_WatcherSettings).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameObjectivesWatcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameObjectivesFacade>().AsSingle().NonLazy();
        }
        #endregion
        #region Inspector
        [SerializeField] private GameObjectivesFacade.Settings _Settings;
        [SerializeField] private GameObjectivesWatcher.Settings _WatcherSettings;
        #endregion
        #region Private Methods
        [Button()]
        private void ResolveObjectivePresets()
        {
            _Settings.ResolveObjectives();
        }
        #endregion
    }
}