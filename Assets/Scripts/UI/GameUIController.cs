using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class GameUIController : MonoBehaviour
    {
        private string menuSceneName = "MenuScene";

        [SerializeField] private GameObject menuUI;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            pauseButton.onClick.AddListener(OnPauseButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            menuUI.SetActive(true);
        }

        private void OnOptionsButtonPressed()
        {
        }

        private void OnExitButtonPressed()
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }
    }
}