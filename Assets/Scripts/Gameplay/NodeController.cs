using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class NodeController
    {
        private int rowCount;
        private int columnCount;
        private Node[][] nodes;
        private Node firstNode;
        private Node secondNode;
        private CrystalFactory crystalFactory;

        public bool IsReady { get; private set; }
        public event Action OnSwap;
        public event Action OnMatch;

        public NodeController(List<GameObject> nodes, int rowCount, int columnCount, CrystalFactory crystalFactory)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.crystalFactory = crystalFactory;
            InitializeNodes(nodes);
            FillNodes();
        }

        public bool ActivateFirstNodeByCrystal(GameObject obj)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (!nodes[i][j].ContainsCrystal(obj)) continue;
                    firstNode = nodes[i][j];
                    firstNode.Activate();
                    return true;
                }
            }
            return false;
        }

        public void ActivateSecondNodeByDirection(Direction dir)
        {
            Node neighbor;
            if (firstNode == null) return;
            bool asd = firstNode.TryGetNeighbor(dir, out neighbor);
            if (asd)
            {
                ChangeSecondActiveNode(neighbor);
            }
        }

        public bool TrySwapActiveCrystals()
        {
            bool result;
            if (firstNode != null && secondNode != null)
            {
                SwapActiveCrystals();
                OnSwap?.Invoke();
                result = true;
            }
            else
                result = false;

            firstNode?.Deactivate();
            secondNode?.Deactivate();
            firstNode = null;
            secondNode = null;
            return result;
        }

        public void RunResolving()
        {
          //  bool isMatch = false;
          //  do
          //  {
          //      FillNodes();
          //      isMatch = CheckField();
          //  }
          //  while (isMatch);
          //  OnMatch?.Invoke();
        }

        public bool IsThereActiveFirst()
        {
            return firstNode != null;
        }

        private void InitializeNodes(List<GameObject> list)
        {
            nodes = new Node[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                nodes[i] = new Node[columnCount];
            }

            int crystalCount = rowCount * columnCount;
            for (int i = 0; i < crystalCount; i++)
            {
                int row = i / columnCount;
                int column = i - rowCount * row;
                Node node = new Node(list[i], crystalFactory);
                nodes[row][column] = node;
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Node node = nodes[i][j];
                    AddNeighbors(node, i, j);
                }
            }
        }

        private void AddNeighbors(Node node, int row, int column)
        {
            if (row > 0)
            {
                var neighbor = nodes[row - 1][column];
                node.TryAddNeighbor(Direction.forward, neighbor);
            }
            if (row < rowCount - 1)
            {
                var neighbor = nodes[row + 1][column];
                node.TryAddNeighbor(Direction.backward, neighbor);
            }
            if (column > 0)
            {
                var neighbor = nodes[row][column - 1];
                node.TryAddNeighbor(Direction.left, neighbor);
            }
            if (column < columnCount - 1)
            {
                var neighbor = nodes[row][column + 1];
                node.TryAddNeighbor(Direction.right, neighbor);
            }
        }

        private void SwapActiveCrystals()
        {
            Crystal crystal1 = firstNode.GetCrystal();
            Crystal crystal2 = secondNode.GetCrystal();

            firstNode.SetCrystal(crystal2);
            secondNode.SetCrystal(crystal1);
        }

        private void ChangeSecondActiveNode(Node newNode)
        {
            secondNode?.Deactivate();
            if (newNode != null)
            {
                secondNode = newNode;
                secondNode.Activate();
            }
        }

        private void FillNodes()
        {
            for (int i = rowCount - 1; i >= 0; i--)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    nodes[i][j].Fill();
                }
            }
        }
    }
}