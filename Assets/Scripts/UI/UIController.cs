using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.UI
{
    public class UIController : MonoBehaviour
    {
        public static event Action OnPlayButtonClicked;

        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameMenuUI;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject optionsUI;
        
        [SerializeField] private Button mainMenuPlayButton;
        [SerializeField] private Button mainMenuOptionsButton;
        [SerializeField] private Button mainMenuExitButton;

        [SerializeField] private Button gameMenuOptionsButton;
        [SerializeField] private Button gameMenuBackButton;
        [SerializeField] private Button gameMenuGoToMenuButton;

        [SerializeField] private Button gamePauseButton;

        [SerializeField] private Button optionsBackButton;

        private void Start()
        {
            mainMenuPlayButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnPlayButtonClick()
        {
            mainMenuUI.SetActive(false);
            gameUI.SetActive(true);
            CameraMover.Instance.GoUp();
            OnPlayButtonClicked?.Invoke();
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }

        private void OnPauseButtonClick()
        {
            // set game on pause
            gameUI.SetActive(false);
            gameMenuUI.SetActive(true);
        }

        private void OnBackButtonClick()
        {
            gameMenuUI.SetActive(false);
            gameUI.SetActive(true);
            // set game on play
        }

        private void OnGoToMenuButtonClick()
        {
            // dispose all current playthrough stuff
            gameMenuUI.SetActive(false);
            mainMenuUI.SetActive(true);
        }
    }
}
