using UnityEngine;
using View.Button;

namespace View.UI.Game
{
    public class DifficultyScreen : UIScreen
    {
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;

        [Space, Header("Buttons")]
        [SerializeField] private CustomButton _back;
        [SerializeField] private CustomButton _easy;
        [SerializeField] private CustomButton _medium;
        [SerializeField] private CustomButton _hard;

        private void OnEnable()
        {
            _back.AddListener(BackToMenu);
            _easy.AddListener(OpenEasy);
            _medium.AddListener(OpenMedium);
            _hard.AddListener(OpenHard);
        }

        private void OnDisable()
        {
            _back.RemoveListener(BackToMenu);
            _easy.RemoveListener(OpenEasy);
            _medium.RemoveListener(OpenMedium);
            _hard.RemoveListener(OpenHard);
        }

        public override void StartScreen()
        {
            base.StartScreen();
        }

        private void BackToMenu()
        {
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.MENU_SCENE);
            CloseScreen();
        }

        private void OpenEasy()
        {
            GameCore.GameManager.Instance.SetDifficulty(0);
        }

        private void OpenMedium()
        {
            GameCore.GameManager.Instance.SetDifficulty(1);
        }

        private void OpenHard()
        {
            GameCore.GameManager.Instance.SetDifficulty(2);
        }
    }
}