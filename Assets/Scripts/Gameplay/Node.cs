using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Node
    {
        private GameObject gameObject;
        private Crystal crystal;
        private CrystalFactory crystalFactory;
        private Dictionary<Direction, Node> neighbors;

        public Node(GameObject gameObject, CrystalFactory crystalFactory)
        {
            this.gameObject = gameObject;
            this.crystalFactory = crystalFactory;
            neighbors = new Dictionary<Direction, Node>(4);
        }

        public void Fill()
        {
            if (crystal != null) return;
            Node node;
            if (neighbors.TryGetValue(Direction.forward, out node) && node.TryGetCrystal(out crystal)) return;
            crystal = crystalFactory.CreateRandomCrystal();
            crystal.ChangePosition(gameObject.transform.position);
        }

        public bool TryAddNeighbor(Direction direction, Node node)
        {
            return neighbors.TryAdd(direction, node);
        }

        public bool TryGetNeighbor(Direction direction, out Node node)
        {
            return neighbors.TryGetValue(direction, out node);
        }

        public bool ContainsCrystal(GameObject obj)
        {
            return crystal.gameObject == obj;
        }

        public void Activate()
        {
            crystal.Activate();
        }

        public void Deactivate()
        {
            crystal.Deactivate();
        }

        public Crystal GetCrystal()
        {
            return crystal;
        }

        public void SetCrystal(Crystal crystal)
        {
            this.crystal = crystal;
            crystal.ChangePosition(gameObject.transform.position);
        }

        private bool TryGetCrystal(out Crystal crystal)
        {
            if (this.crystal != null)
            {
                crystal = this.crystal;
                return true;
            }

            Node node;
            if (neighbors.TryGetValue(Direction.forward, out node))
            {
                return node.TryGetCrystal(out crystal);
            }
            crystal = null;
            return false;
        }
    }
}
