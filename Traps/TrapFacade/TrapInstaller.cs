namespace Muciojad.SpaceHorror.Gameplay.Traps.TrapFacade
{
    using Zenject;

    public class TrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TrapFacade>().AsSingle().NonLazy();
        }
    }
}