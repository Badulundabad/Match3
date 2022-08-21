using Match3.Gameplay;
using Match3.Audio;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject redCrystal;
        [SerializeField] private GameObject greenCrystal;
        [SerializeField] private GameObject blueCrystal;
        [SerializeField] private GameObject pinkCrystal;
        [SerializeField] private GameObject yellowCrystal;
        [SerializeField] private GameObject nodeField;
        [SerializeField] private AudioList audioList;
        private int rowCount = 4;
        private int columnCount = 4;
        private GameObject[] nodeObjects;
        private Dictionary<CrystalColor, GameObject> prefabs;

        public override void InstallBindings()
        {
            Container.Bind<IInputHelper>().To<MouseHelper>().AsSingle();
            BindAudioPlayer();
            BindCrystalFactory();
            BindNodeField();
        }

        private void BindAudioPlayer()
        {
            Container.Bind<AudioList>().FromNewScriptableObject(audioList).AsSingle();
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioPlayer>().FromComponentInHierarchy().AsSingle();
        }

        private void BindCrystalFactory()
        {
            prefabs = new Dictionary<CrystalColor, GameObject>();
            prefabs.Add(CrystalColor.red, redCrystal);
            prefabs.Add(CrystalColor.green, greenCrystal);
            prefabs.Add(CrystalColor.blue, blueCrystal);
            prefabs.Add(CrystalColor.pink, pinkCrystal);
            prefabs.Add(CrystalColor.yellow, yellowCrystal);
            Container.Bind<Dictionary<CrystalColor, GameObject>>().FromInstance(prefabs).WhenInjectedInto<CrystalFactory>();
            Container.Bind<CrystalFactory>().AsSingle();
        }

        private void BindNodeField()
        {
            nodeObjects = GetNodeObjects();
            Container.Bind<GameObject[]>().FromInstance(nodeObjects).WhenInjectedInto<NodeField>();
            Container.Bind<int>().WithId("rows").FromInstance(rowCount).WhenInjectedInto<NodeField>();
            Container.Bind<int>().WithId("columns").FromInstance(columnCount).WhenInjectedInto<NodeField>();
            Container.Bind<NodeField>().AsSingle();
        }

        private GameObject[] GetNodeObjects()
        {
            int nodeCount = rowCount * columnCount;
            GameObject[] objects = new GameObject[nodeCount];
            for (int i = 0; i < nodeCount; i++)
            {
                objects[i] = nodeField.transform.GetChild(i).gameObject;
            }
            return objects;
        }
    }
}