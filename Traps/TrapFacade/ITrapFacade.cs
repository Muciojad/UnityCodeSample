namespace Muciojad.SpaceHorror.Gameplay.Traps
{
    using System;
    using TrapFacade;

    public interface ITrapFacade
    {
        void RegisterTrap(ITrap trap);
        void UnregisterTrap(ITrap trap);
    }
}