using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        private MouseHandler mouseHandler;
        private NodeController nodeController;
        private GameObject pickedObject;

        private void Start()
        {
            mouseHandler = new MouseHandler();
            nodeController = GetComponent<NodeController>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (pickedObject == null)
                {
                    if (mouseHandler.TryToPick(out pickedObject))
                    {
                        nodeController.SetCrystalActive(pickedObject);
                    }
                }
                else
                {
                    Direction dir = mouseHandler.GetMoveDirection(pickedObject.transform.position);
                    nodeController.SearchCrystalToSwap(dir);
                    Cursor.visible = false;
                }
            }
            else
            {
                if (pickedObject != null)
                {
                    nodeController.TryToSwap();
                }
                pickedObject = null;
                Cursor.visible = true;
            }
        }
    }
}