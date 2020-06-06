namespace Muciojad.SpaceHorror.Systems.GameObjectives.Components
{
    using System;
    using UnityEngine;
    using Zenject;

    public class FindAccessCardConsoleDoorPresenter : GameObjectivePresenter
    {
        #region Inspector
        [SerializeField] private PermissionSystem.PermissionType _RequiredPermission;
        #endregion
        #region Unity Methods
        private void OnEnable()
        {
            _PlayerPermission.PermissionChanged += RefreshObjective;
        }
        private void OnDisable()
        {
            _PlayerPermission.PermissionChanged -= RefreshObjective;
        }
        #endregion
        #region Private Variables
        [Inject] private PlayerPermission _PlayerPermission;
        #endregion
        #region Private Methods
        private void RefreshObjective(PermissionSystem.PermissionType permissionType)
        {
            if(permissionType >= _RequiredPermission)
            {
                HandleTaskFinished();
            }
        }
        #endregion
    }
}