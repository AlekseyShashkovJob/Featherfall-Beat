using UnityEngine;
using View.Button;

namespace View.UI.Game
{
    public class GameplayScreen : UIScreen
    {
        [SerializeField] private CustomButton _pause;
        [SerializeField] private UIScreen _pauseScreen;

        private void OnEnable()
        {
            _pause.AddListener(PauseGame);
        }

        private void OnDisable()
        {
            _pause.RemoveListener(PauseGame);
        }

        private void PauseGame()
        {
            _pauseScreen.StartScreen();
        }
    }
}