using System;
using UnityEngine;
using View.Button;
using UnityEngine.UI;

namespace View.UI.Menu
{
    public class OptionsScreen : UIScreen
    {
        [SerializeField] private CustomButton _back;

        [SerializeField] private CustomButton _sound;
        [SerializeField] private CustomButton _vibro;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Image _vibroImage;
        [SerializeField] private Image _soundTextImage;
        [SerializeField] private Image _vibroTextImage;

        [SerializeField] private Sprite _spriteSoundOn;
        [SerializeField] private Sprite _spriteSoundOff;
        [SerializeField] private Sprite _spriteVibroOn;
        [SerializeField] private Sprite _spriteVibroOff;
        [SerializeField] private Sprite _spriteSoundTextOn;
        [SerializeField] private Sprite _spriteSoundTextOff;
        [SerializeField] private Sprite _spriteVibroTextOn;
        [SerializeField] private Sprite _spriteVibroTextOff;

        private bool _isSoundOn = true;
        private bool _isVibroOn = false;
		
		public static Action OnSoundStateChanged;
        public static Action OnVibroStateChanged;

        private void OnEnable()
        {
            _back.AddListener(BackToMenu);
            _sound.AddListener(ToggleSoundImage);
            _vibro.AddListener(ToggleVibroImage);
			
			_isSoundOn = PlayerPrefs.GetInt(Misc.Services.PlayerPrefsKeys.SoundOn, 1) == 1;
            _isVibroOn = PlayerPrefs.GetInt(Misc.Services.PlayerPrefsKeys.VibroOn, 0) == 1;
			
			UpdateSoundUI();
            UpdateVibroUI();
        }

        private void OnDisable()
        {
            _back.RemoveListener(BackToMenu);
            _sound.RemoveListener(ToggleSoundImage);
            _vibro.RemoveListener(ToggleVibroImage);
        }

        private void BackToMenu()
        {
            CloseScreen();
        }

        private void ToggleSoundImage()
        {
            _isSoundOn = !_isSoundOn;
            PlayerPrefs.SetInt(Misc.Services.PlayerPrefsKeys.SoundOn, _isSoundOn ? 1 : 0);
            UpdateSoundUI();
            OnSoundStateChanged?.Invoke();
        }

        private void ToggleVibroImage()
        {
            _isVibroOn = !_isVibroOn;
            PlayerPrefs.SetInt(Misc.Services.PlayerPrefsKeys.VibroOn, _isVibroOn ? 1 : 0);
            UpdateVibroUI();
            OnVibroStateChanged?.Invoke();
        }
		
		private void UpdateSoundUI()
        {
            _soundImage.sprite = _isSoundOn ? _spriteSoundOn : _spriteSoundOff;
            _soundTextImage.sprite = _isSoundOn ? _spriteSoundTextOn : _spriteSoundTextOff;
        }

        private void UpdateVibroUI()
        {
            _vibroImage.sprite = _isVibroOn ? _spriteVibroOn : _spriteVibroOff;
            _vibroTextImage.sprite = _isVibroOn ? _spriteVibroTextOn : _spriteVibroTextOff;
        }
    }
}