using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MenuUIController : MonoBehaviour
    {
        private string gameSceneName = "GameScene";
        public Button playButton;
        public Button optionsButton;
        public Button exitButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnPlayButtonPressed()
        {
            SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        }
        private void OnOptionsButtonPressed()
        {
        }
        private void OnExitButtonPressed()
        {
            Application.Quit();
        }
    }
}