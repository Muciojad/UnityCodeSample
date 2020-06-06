namespace Muciojad.SpaceHorror.Gameplay.Traps
{
    using System;
    using TrapFacade;

    public interface ITrapRequestHandler
    {
        void RequestTrap(TrapType trapType);
    }
}