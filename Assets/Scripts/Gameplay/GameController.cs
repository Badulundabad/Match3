using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        private int rowCount = 4;
        private int columnCount = 4;
        private Vector3[][] crystalPositions;
        private Crystal[][] crystals;
        private Crystal firstCrystal;
        private Crystal secondCrystal;
        private GameObject pickedObject;
        private MouseHandler mouseHandler;

        [SerializeField] private GameObject redCrystal;
        [SerializeField] private GameObject greenCrystal;
        [SerializeField] private GameObject blueCrystal;
        [SerializeField] private GameObject pinkCrystal;
        [SerializeField] private GameObject yellowCrystal;
        private GameObject[] prefabs;

        private void Start()
        {
            mouseHandler = new MouseHandler();
            InitializeArrays();
            FillFieldWithCrystals();
        }

        private void InitializeArrays()
        {
            crystalPositions = new Vector3[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                crystalPositions[i] = new Vector3[columnCount];
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
        }

        private void FillFieldWithCrystals()
        {
            int crystalCount = rowCount * columnCount;
            for (int i = 0; i < crystalCount; i++)
            {
                int row = i / 4;
                int column = i - 4 * row;

                Transform child = transform.GetChild(i);
                crystalPositions[row][column] = child.position;

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

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (firstCrystal == null)
                {
                    if (mouseHandler.TryToPick(out pickedObject))
                    {
                        firstCrystal = FindCrystalByObject(pickedObject);
                        firstCrystal.Activate();
                    }
                }
                else
                {
                    Direction dir = mouseHandler.GetMoveDirection(pickedObject.transform.position);
                    Crystal crystal = FindSecondCrystalByDirection(dir);
                    ChangeSecondCrystal(crystal);
                    Cursor.visible = false;
                }
            }
            else
            {
                if (pickedObject != null)
                {
                    OnDrop();
                }
                pickedObject = null;
                Cursor.visible = true;
            }
        }

        private Crystal FindCrystalByObject(GameObject obj)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (crystals[i][j].gameObject == obj)
                    {
                        return crystals[i][j];
                    }
                }
            }
            return null;
        }

        private Crystal FindSecondCrystalByDirection(Direction dir)
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

        private void ChangeSecondCrystal(Crystal crystal)
        {
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

        private void OnDrop()
        {
            if (firstCrystal != null && secondCrystal != null)
            {
                SwapCrystals();
            }
            else
            {
                firstCrystal?.Deactivate();
                secondCrystal?.Deactivate();
            }
            firstCrystal = null;
            secondCrystal = null;
        }

        private void SwapCrystals()
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
    }
}