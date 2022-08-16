using Scripts.Audio;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        private GameObject pickedObject;
        private IInputHelper inputHandler;
        private AudioPlayer audioPlayer;
        private NodeField nodeField;

        [Inject]
        private void Construct(IInputHelper inputHandler, NodeField nodeField, AudioPlayer audioPlayer)
        {
            this.inputHandler = inputHandler;
            this.audioPlayer = audioPlayer;
            this.nodeField = nodeField;
            nodeField.OnCrystalsSwap += () => this.audioPlayer.PlayCrystalSwap();
            nodeField.OnCrystalsMatch += (amount) => OnCrystalsMatch(amount);
            UIController.OnPlayButtonClicked += () => OnPlayButtonClicked();
        }

        private void OnDestroy()
        {
            nodeField.OnCrystalsSwap -= () => audioPlayer.PlayCrystalSwap();
            nodeField.OnCrystalsMatch -= (amount) => OnCrystalsMatch(amount);
            UIController.OnPlayButtonClicked -= () => OnPlayButtonClicked();
        }

        private void Update()
        {
            if (!nodeField.IsReady) return;

            if (Input.GetMouseButton(0))
            {
                Cursor.visible = false;
                if (pickedObject == null && inputHandler.TryToPick(out pickedObject))
                {
                    nodeField.ActivateFirstNodeByCrystal(pickedObject);
                }
                Direction dir = inputHandler.GetSwapDirection();
                nodeField.ActivateSecondNodeByDirection(dir);
            }
            else
            {
                Cursor.visible = true;
                if (pickedObject == null) return;

                pickedObject = null;
                if (nodeField.TrySwapActiveCrystals())
                {
                    StartCoroutine(nodeField.RunResolvingAsync(true));
                }
            }
        }

        private void OnCrystalsMatch(int amount)
        {
            audioPlayer.PlayCrystalMatch();
        }

        private void OnPlayButtonClicked()
        {
            StartCoroutine(nodeField.RunResolvingAsync(false));
        }
    }
}