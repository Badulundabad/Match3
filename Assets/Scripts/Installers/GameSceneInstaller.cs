using Scripts.Gameplay;
using Scripts.Audio;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private AudioClip victory;
        [SerializeField] private AudioClip swap;
        [SerializeField] private AudioClip match;
        [SerializeField] private AudioClip reject;
        [SerializeField] private GameObject redCrystal;
        [SerializeField] private GameObject greenCrystal;
        [SerializeField] private GameObject blueCrystal;
        [SerializeField] private GameObject pinkCrystal;
        [SerializeField] private GameObject yellowCrystal;
        private Dictionary<CrystalColor, GameObject> prefabs;


        public override void InstallBindings()
        {
            BindAudio();
            BindCrystals();
        }

        private void BindAudio()
        {
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioClip>().WithId("victory").FromInstance(victory);
            Container.Bind<AudioClip>().WithId("swap").FromInstance(swap);
            Container.Bind<AudioClip>().WithId("match").FromInstance(match);
            Container.Bind<AudioClip>().WithId("reject").FromInstance(reject);
            Container.Bind<AudioPlayer>().AsSingle();
        }

        private void BindCrystals()
        {
            prefabs = new Dictionary<CrystalColor, GameObject>();
            prefabs.Add(CrystalColor.red, redCrystal);
            prefabs.Add(CrystalColor.green, greenCrystal);
            prefabs.Add(CrystalColor.blue, blueCrystal);
            prefabs.Add(CrystalColor.pink, pinkCrystal);
            prefabs.Add(CrystalColor.yellow, yellowCrystal);
            Container.Bind<Dictionary<CrystalColor, GameObject>>().FromInstance(prefabs).WhenInjectedInto<CrystalFactory>();
            Container.Bind<CrystalFactory>().AsSingle();
            Container.Bind<IInputHandler>().To<MouseHandler>().AsSingle();
        }
    }
}