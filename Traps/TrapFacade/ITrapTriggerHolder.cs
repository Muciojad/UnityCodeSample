namespace Muciojad.SpaceHorror.Gameplay.Traps
{
    using System;
    using TrapFacade;

    public interface ITrapTriggerHolder
    {
        void RegisterTrapTrigger(TrapTriggerComponent triggerComponent);
        void UnregisterTrapTrigger(TrapTriggerComponent triggerComponent);
    }
}