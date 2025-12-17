using System;
using View.Button;
using UnityEngine;

namespace View.UI.Menu
{
    public class NoInternetScreen : UIScreen
    {
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;
		[SerializeField] private GameObject _backgroundPortrait;
        [SerializeField] private GameObject _backgroundLandscape;
        [SerializeField] private CustomButton _backPortrait;
        [SerializeField] private CustomButton _backLandscape;

        private Vector2Int _lastResolution;

        private void OnEnable()
        {
            _backPortrait.AddListener(Restart);
            _backLandscape.AddListener(Restart);
        }

        private void OnDisable()
        {
            _backPortrait.RemoveListener(Restart);
            _backLandscape.RemoveListener(Restart);
        }

        private void Update()
        {
            Vector2Int currentResolution = new Vector2Int(Screen.width, Screen.height);
            if (currentResolution != _lastResolution)
            {
                _lastResolution = currentResolution;
                UpdateBackground();
            }
        }

        public override void StartScreen()
        {
            base.StartScreen();

            _lastResolution = new Vector2Int(Screen.width, Screen.height);
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            bool isPortrait = Screen.height >= Screen.width;

            _backgroundPortrait.SetActive(isPortrait);
            _backgroundLandscape.SetActive(!isPortrait);
        }

        private void Restart()
        {
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.MENU_SCENE);
            CloseScreen();
        }
    }
}