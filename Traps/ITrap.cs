namespace Muciojad.SpaceHorror.Gameplay.Traps
{
    using System;
    using TrapFacade;

    public interface ITrap
    {
        TrapType TrapType { get; }
        void Activate();
        void OnDeactivate();

        event Action Activated;
        event Action Deactivated;
    }
}