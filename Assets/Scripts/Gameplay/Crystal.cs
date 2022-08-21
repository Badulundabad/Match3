using UnityEngine;

namespace Match3.Gameplay
{
    public class Crystal
    {
        public Vector3 position { get; private set; }
        public CrystalColor color { get; private set; }
        public GameObject gameObject { get; private set; }

        public Crystal(CrystalColor color, GameObject gameObject)
        {
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

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
