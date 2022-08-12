using Scripts.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject redCrystal;
        [SerializeField] private GameObject greenCrystal;
        [SerializeField] private GameObject blueCrystal;
        [SerializeField] private GameObject pinkCrystal;
        [SerializeField] private GameObject yellowCrystal;
        private Dictionary<CrystalColor, GameObject> prefabs;

        public override void InstallBindings()
        {
            prefabs = new Dictionary<CrystalColor, GameObject>();
            prefabs.Add(CrystalColor.red, redCrystal);
            prefabs.Add(CrystalColor.green, greenCrystal);
            prefabs.Add(CrystalColor.blue, blueCrystal);
            prefabs.Add(CrystalColor.pink, pinkCrystal);
            prefabs.Add(CrystalColor.yellow, yellowCrystal);
            Container.Bind<Dictionary<CrystalColor, GameObject>>().FromInstance(prefabs).WhenInjectedInto<CrystalFactory>();
            Container.Bind<CrystalFactory>().AsSingle();
            Container.Bind<MouseHandler>().AsSingle();
        }
    }
}