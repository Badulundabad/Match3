using UnityEngine;

namespace Scripts.Gameplay
{
    public class Crystal
    {
        public int row { get; private set; }
        public int column { get; private set; }
        public Vector3 position { get; private set; }
        public CrystalColor color { get; private set; }
        public GameObject gameObject { get; private set; }

        public Crystal(CrystalColor color, GameObject gameObject)
        {
            this.color = color;
            this.gameObject = gameObject;
        }

        public Crystal(int row, int column, CrystalColor color,GameObject gameObject)
        {
            this.row = row;
            this.column = column;
            this.color = color;
            this.gameObject = gameObject;
        }

        public void Activate()
        {
            gameObject.transform.position = position + new Vector3(0, 5f, 0);
        }

        public void Deactivate()
        {
            gameObject.transform.position = position;
        }

        public void ChangePosition(Vector3 position)
        {
            this.position = position;
            gameObject.transform.position = position;
        }

        public void ChangeNode(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
