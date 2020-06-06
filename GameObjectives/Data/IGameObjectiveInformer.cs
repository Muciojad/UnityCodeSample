namespace Muciojad.SpaceHorror.Data.Objectives
{
    public interface IGameObjectiveInformer
    {
        void OnProgressed(int amount);
        void Finished();
    }
}