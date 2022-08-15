using Scripts.Audio;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        private int rowCount = 4;
        private int columnCount = 4;
        private GameObject pickedObject;
        private IInputHelper inputHandler;
        private AudioPlayer audioPlayer;
        private NodeController nodeController;

        [Inject]
        private void Construct(IInputHelper inputHandler, CrystalFactory crystalFactory, AudioPlayer audioPlayer)
        {
            this.inputHandler = inputHandler;
            this.audioPlayer = audioPlayer;
            nodeController = new NodeController(GetNodeList(), rowCount, columnCount, crystalFactory);
            nodeController.OnSwap += () => this.audioPlayer.PlayCrystalSwap();
            nodeController.OnMatch += () => this.audioPlayer.PlayCrystalMatch();
        }

        private void OnDestroy()
        {
            nodeController.OnSwap -= () => this.audioPlayer.PlayCrystalSwap();
            nodeController.OnMatch -= () => this.audioPlayer.PlayCrystalMatch();
        }

        private List<GameObject> GetNodeList()
        {
            List<GameObject> list = new List<GameObject>();
            int crystalCount = rowCount * columnCount;
            for (int i = 0; i < crystalCount; i++)
            {
                list.Add(transform.GetChild(i).gameObject);
            }
            return list;
        }

        private void Update()
        {
            if (!nodeController.IsReady) return;

            if (Input.GetMouseButton(0))
            {
                Cursor.visible = false;
                if (pickedObject == null && inputHandler.TryToPick(out pickedObject))
                {
                    nodeController.ActivateFirstNodeByCrystal(pickedObject);
                }
                Direction dir = inputHandler.GetSwapDirection();
                nodeController.ActivateSecondNodeByDirection(dir);
            }
            else
            {
                Cursor.visible = true;
                if (pickedObject == null) return;

                pickedObject = null;
                if (nodeController.TrySwapActiveCrystals())
                {
                    //nodeController.RunResolving();
                }
            }
        }
    }
}