namespace Muciojad.SpaceHorror.Systems.GameObjectives
{
    using Data.Objectives;

    public interface IGameObjectivesAmountChangedReceiver
    {
        void NotifyObjectiveCollectedAmountChanged(GameObjective objective, int newAmount);
    }
}