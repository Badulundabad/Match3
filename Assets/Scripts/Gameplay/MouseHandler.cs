using UnityEngine;

namespace Scripts.Gameplay
{
    public class MouseHandler : IInputHandler
    {
        private const int pickableLayer = 6;
        private GameObject selectedObject;

        public bool TryToPick(out GameObject obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == pickableLayer)
                {
                    selectedObject = hit.collider.gameObject.transform.parent.gameObject;
                    obj = selectedObject;
                    return true;
                }
            }
            selectedObject = null;
            obj = selectedObject;
            return false;
        }

        public Direction GetMoveDirection(Vector3 position)
        {
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);
            Vector3 vector = (worldPosition - position).normalized;

            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
            {
                if (vector.x > 0)
                    return Direction.right;
                else
                    return Direction.left;
            }
            else if (vector.z < 0)
                return Direction.backward;
            else
                return Direction.forward;
        }
    }
}