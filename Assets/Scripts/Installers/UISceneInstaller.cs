using Match3.UI;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    public class UISceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform bottomPoint;
        [SerializeField] private Transform upperPoint;        

        public override void InstallBindings()
        {
            Container.Bind<Transform>().WithId("bottom").FromInstance(bottomPoint).WhenInjectedInto<CameraMover>();
            Container.Bind<Transform>().WithId("upper").FromInstance(upperPoint).WhenInjectedInto<CameraMover>();
            Container.Bind<CameraMover>().FromComponentInHierarchy().AsSingle();
        }
    }
}