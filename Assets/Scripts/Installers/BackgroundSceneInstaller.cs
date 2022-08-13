using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class BackgroundSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
        }
    }
}