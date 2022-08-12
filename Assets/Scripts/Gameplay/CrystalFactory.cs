using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Gameplay
{
    public class CrystalFactory
    {
        [Inject]
        private Dictionary<CrystalColor, GameObject> prefabs;

        public Crystal CreateRandomCrystal()
        {
            int index = Random.Range(0, prefabs.Count);
            GameObject obj = GameObject.Instantiate(prefabs[(CrystalColor)index]);
            return new Crystal((CrystalColor)index, obj);
        }

        public Crystal CreateCrystalByColor(CrystalColor color)
        {
            GameObject obj = GameObject.Instantiate(prefabs[color]);
            return new Crystal(color, obj);
        }
    }
}