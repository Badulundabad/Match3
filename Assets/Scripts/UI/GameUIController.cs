using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class GameUIController : MonoBehaviour
    {
        private string menuSceneName = "MenuScene";
        public Button pauseButton;
        public Button optionsButton;

        private void Start()
        {
            pauseButton.onClick.AddListener(OnPauseButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }

        private void OnOptionsButtonPressed()
        {
        }
    }
}