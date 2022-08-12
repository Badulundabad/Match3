using UnityEngine;

namespace Scripts.Gameplay
{
    public class NodeController : MonoBehaviour
    {
        private int rowCount = 4;
        private int columnCount = 4;
        private Transform[][] nodes;
        private Crystal[][] crystals;
        private Crystal firstCrystal;
        private Crystal secondCrystal;

        [SerializeField] private GameObject redCrystal;
        [SerializeField] private GameObject greenCrystal;
        [SerializeField] private GameObject blueCrystal;
        [SerializeField] private GameObject pinkCrystal;
        [SerializeField] private GameObject yellowCrystal;
        private GameObject[] prefabs;

        void Start()
        {
            nodes = new Transform[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                nodes[i] = new Transform[columnCount];
            }

            crystals = new Crystal[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                crystals[i] = new Crystal[columnCount];
            }

            prefabs = new GameObject[5];
            prefabs[0] = redCrystal;
            prefabs[1] = greenCrystal;
            prefabs[2] = blueCrystal;
            prefabs[3] = pinkCrystal;
            prefabs[4] = yellowCrystal;

            FillNodes();
        }

        public void SetCrystalActive(GameObject obj)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (crystals[i][j].gameObject == obj)
                    {
                        firstCrystal = crystals[i][j];
                        firstCrystal.Activate();
                        break;
                    }
                }
            }
        }

        public void SearchCrystalToSwap(Direction dir)
        {
            Crystal crystal = GetCrystalFromDirection(dir);

            if (crystal != null)
            {
                if (secondCrystal != null && secondCrystal != crystal)
                {
                    secondCrystal.Deactivate();
                }
                secondCrystal = crystal;
                secondCrystal.Activate();
            }
            else if (secondCrystal != null)
            {
                secondCrystal.Deactivate();
                secondCrystal = null;
            }
        }

        public void TryToSwap()
        {
            if (firstCrystal != null && secondCrystal != null)
            {
                Swap();
            }
            else
            {
                firstCrystal?.Deactivate();
                secondCrystal?.Deactivate();
            }
            firstCrystal = null;
            secondCrystal = null;
        }

        private void Swap()
        {
            int row = firstCrystal.row;
            int column = firstCrystal.column;
            Vector3 position = firstCrystal.position;

            int row2 = secondCrystal.row;
            int column2 = secondCrystal.column;
            Vector3 position2 = secondCrystal.position;

            firstCrystal.ChangeNode(row2, column2);
            firstCrystal.ChangePosition(position2);
            crystals[row2][column2] = firstCrystal;

            secondCrystal.ChangeNode(row, column);
            secondCrystal.ChangePosition(position);
            crystals[row][column] = secondCrystal;
        }

        private Crystal GetCrystalFromDirection(Direction dir)
        {
            int row = firstCrystal.row;
            int column = firstCrystal.column;
            if (dir == Direction.forward)
                row--;
            else if (dir == Direction.backward)
                row++;
            else if (dir == Direction.left)
                column--;
            else
                column++;

            bool isIndexesValid = column > -1 && column < columnCount && row > -1 && row < rowCount;
            return isIndexesValid ? crystals[row][column] : null;
        }

        private void FillNodes()
        {
            int crystalCount = rowCount * columnCount;
            for (int i = 0; i < crystalCount; i++)
            {
                int row = i / 4;
                int column = i - 4 * row;

                Transform child = transform.GetChild(i);
                nodes[row][column] = child;

                Crystal crystal = GenerateCrystal();
                crystal.ChangePosition(child.position);
                crystal.ChangeNode(row, column);
                crystals[row][column] = crystal;
            }
        }

        private Crystal GenerateCrystal()
        {
            int index = Random.Range(0, 4);
            GameObject obj = Instantiate(prefabs[index]);
            return new Crystal((CrystalColor)index, obj);
        }
    }
}