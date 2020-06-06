namespace Muciojad.SpaceHorror.Systems.GameObjectives.Components
{
    using System;
    using Gameplay.PhotoMode;
    using Gameplay.PhotoMode.Interfaces;
    using UnityEngine;
    using Zenject;

    public class TakeCrawlerPhotoObjectivePresenter : GameObjectivePresenter
    {
        #region Inspector
        [SerializeField] private PhotoTargetType _PhotoTargetType;
        #endregion
        #region Unity Methods
        private void OnEnable()
        {
            _PhotoModeEventDispatcher.PhotoTaken += CheckPhoto;
        }
        private void OnDisable()
        {
            _PhotoModeEventDispatcher.PhotoTaken -= CheckPhoto;
        }
        #endregion
        #region Private Variables
        [Inject] private IPhotoModeEventDispatcher _PhotoModeEventDispatcher;
        private bool _Finished;
        #endregion
        #region Private Methods
        private void CheckPhoto(PhotoResult photoResult)
        {
            if (_Finished) return;
            if (photoResult.TargetType != _PhotoTargetType || photoResult.DistanceAndSize != PhotoResult.PhotoDistanceAndSize.Proper)
            {
                return;
            }
            HandleTaskFinished();
            _Finished = true;
        }
        #endregion
    }
}