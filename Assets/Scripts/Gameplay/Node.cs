using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Match3.Gameplay
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
            if (crystal != null)
            {
                return;
            }
            if (!TryReturnCrystal(out crystal))
            {
                crystal = crystalFactory.CreateRandomCrystal();
            }
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

        public void ActivateCrystal()
        {
            crystal.Activate();
        }

        public void DeactivateCrystal()
        {
            crystal.Deactivate();
        }

        public bool ContainsCrystal(GameObject obj)
        {
            return crystal.gameObject == obj;
        }

        public void SetCrystal(Crystal crystal)
        {
            this.crystal = crystal;
            crystal.ChangePosition(gameObject.transform.position);
        }

        public Crystal GetCrystal()
        {
            return crystal;
        }

        public CrystalColor GetCrystalColor()
        {
            return crystal != null ? crystal.color : CrystalColor.none;
        }

        public void DestroyCrystal()
        {
            if (crystal != null)
            {
                crystal.Destroy();
                crystal = null;
            }
        }

        private bool TryReturnCrystal(out Crystal crystal)
        {
            if (this.crystal != null)
            {
                crystal = this.crystal;
                this.crystal = null;
                return true;
            }

            Node neighbor;
            if (neighbors.TryGetValue(Direction.forward, out neighbor) && neighbor.TryReturnCrystal(out crystal))
            {
                return true;
            }
            crystal = null;
            return false;
        }

    }
}
