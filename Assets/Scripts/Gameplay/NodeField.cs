using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Match3.Gameplay
{
    public class NodeField
    {
        private const int minMatchCount = 2;

        private bool isCallbackNeeded;
        private int rowCount;
        private int columnCount;
        private Node[][] nodes;
        private Node firstNode;
        private Node secondNode;

        public bool IsReady { get; private set; }
        public event Action OnCrystalsSwap;
        public event Action<int> OnCrystalsMatch;

        [Inject]
        public NodeField(GameObject[] nodes, [Inject(Id = "rows")] int rows, [Inject(Id = "columns")] int columns, CrystalFactory crystalFactory)
        {
            rowCount = rows;
            columnCount = columns;
            InitializeNodes(nodes, crystalFactory);
            IsReady = true;
        }

        public bool ActivateFirstNodeByCrystal(GameObject obj)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (!nodes[i][j].ContainsCrystal(obj)) continue;
                    firstNode = nodes[i][j];
                    firstNode.ActivateCrystal();
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
                result = true;
            }
            else
                result = false;

            firstNode?.DeactivateCrystal();
            secondNode?.DeactivateCrystal();
            firstNode = null;
            secondNode = null;
            return result;
        }

        public IEnumerator RunResolvingAsync(bool isCallbackNeeded)
        {
            IsReady = false;
            bool isMatch = true;
            this.isCallbackNeeded = isCallbackNeeded;
            while (isMatch)
            {
                FillNodes();
                isMatch = CheckFieldForMatch();
                yield return null;
            }
            this.isCallbackNeeded = true;
            IsReady = true;
        }

        public bool IsThereActiveFirst()
        {
            return firstNode != null;
        }

        private void InitializeNodes(GameObject[] nodeObjects, CrystalFactory crystalFactory)
        {
            nodes = new Node[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                nodes[i] = new Node[columnCount];
            }

            int nodeCount = rowCount * columnCount;
            for (int i = 0; i < nodeCount; i++)
            {
                int row = i / columnCount;
                int column = i - rowCount * row;
                Node node = new Node(nodeObjects[i], crystalFactory);
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

            if (isCallbackNeeded)
                OnCrystalsSwap?.Invoke();
        }

        private void ChangeSecondActiveNode(Node newNode)
        {
            secondNode?.DeactivateCrystal();
            if (newNode != null)
            {
                secondNode = newNode;
                secondNode.ActivateCrystal();
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

        private bool CheckFieldForMatch()
        {
            bool result = false;
            for (int i = 0; i < rowCount; i++)
            {
                result |= TryToDestroyRow(i);
            }
            for (int j = 0; j < columnCount; j++)
            {
                result |= TryToDestroyColumn(j);
            }

            return result;
        }

        private bool TryToDestroyRow(int index)
        {
            CrystalColor color = nodes[index][0].GetCrystalColor();
            int startIndex = -1;
            int matchCount = 1;

            for (int i = 1; i < columnCount; i++)
            {
                Node node = nodes[index][i];
                if (color == node.GetCrystalColor())
                {
                    matchCount++;

                    if (startIndex < 0)
                    {
                        startIndex = i - 1;
                    }

                    if (i == columnCount - 1 && matchCount > minMatchCount)
                    {
                        DestroyRow(index, startIndex, matchCount);
                        return true;
                    }
                }
                else
                {
                    if (matchCount > minMatchCount)
                    {
                        DestroyRow(index, startIndex, matchCount);
                        return true;
                    }
                    startIndex = -1;
                    matchCount = 1;
                }
                color = node.GetCrystalColor();
            }
            return false;
        }

        private void DestroyRow(int row, int startIndex, int matchCount)
        {
            int num = startIndex + matchCount;
            for (int i = startIndex; i < num; i++)
            {
                Node node = nodes[row][i];
                node.DestroyCrystal();
            }
            if (isCallbackNeeded)
                OnCrystalsMatch?.Invoke(matchCount);
        }

        private bool TryToDestroyColumn(int index)
        {
            CrystalColor color = nodes[0][index].GetCrystalColor();
            int startIndex = -1;
            int matchCount = 1;

            for (int i = 1; i < rowCount; i++)
            {
                Node node = nodes[i][index];
                if (color == node.GetCrystalColor())
                {
                    matchCount++;

                    if (startIndex < 0)
                    {
                        startIndex = i - 1;
                    }

                    if (i == rowCount - 1 && matchCount > minMatchCount)
                    {
                        DestroyColumn(index, startIndex, matchCount);
                        return true;
                    }
                }
                else
                {
                    if (matchCount > minMatchCount)
                    {
                        DestroyColumn(index, startIndex, matchCount);
                        return true;
                    }
                    startIndex = -1;
                    matchCount = 1;
                }
                color = node.GetCrystalColor();
            }
            return false;
        }

        private void DestroyColumn(int column, int startIndex, int matchCount)
        {
            int num = startIndex + matchCount;
            for (int i = startIndex; i < num; i++)
            {
                Node node = nodes[i][column];
                node.DestroyCrystal();
            }
            if (isCallbackNeeded)
                OnCrystalsMatch?.Invoke(matchCount);
        }
    }
}