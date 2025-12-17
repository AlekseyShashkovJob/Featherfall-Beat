using System.Collections;
using TMPro;
using UnityEngine;

namespace View.UI.Shared
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private const string BASE_TEXT = "Loading";
        private const string DOTS = "...";

        [SerializeField] private GameObject _backgroundPortrait;
        [SerializeField] private GameObject _backgroundLandscape;
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private float dotAnimSpeed = 0.3f;

        private Coroutine _dotAnimCoroutine;
        private Coroutine _screenCheckCoroutine;
        private Vector2 _lastScreenSize;

        void OnEnable()
        {
            _loadingText.text = BASE_TEXT + DOTS;
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
            UpdateLogo();

            _dotAnimCoroutine = StartCoroutine(AnimateDots());
            _screenCheckCoroutine = StartCoroutine(CheckScreenSize());
        }

        void OnDisable()
        {
            if (_dotAnimCoroutine != null)
                StopCoroutine(_dotAnimCoroutine);

            if (_screenCheckCoroutine != null)
                StopCoroutine(_screenCheckCoroutine);
        }

        private IEnumerator AnimateDots()
        {
            yield return new WaitForSeconds(0.5f);

            int dotCount = 0;
            while (true)
            {
                dotCount = (dotCount + 1) % 4;
                string currentDots = new string('.', dotCount);
                _loadingText.text = BASE_TEXT + currentDots;
                yield return new WaitForSeconds(dotAnimSpeed);

                if (dotCount == 3)
                    yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator CheckScreenSize()
        {
            while (true)
            {
                if (ScreenSizeChanged())
                {
                    UpdateLogo();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private bool ScreenSizeChanged()
        {
            Vector2 currentSize = new Vector2(Screen.width, Screen.height);
            if (currentSize != _lastScreenSize)
            {
                _lastScreenSize = currentSize;
                return true;
            }
            return false;
        }

        private void UpdateLogo()
        {
            bool isPortrait = Screen.height >= Screen.width;

            _backgroundPortrait.SetActive(isPortrait);
            _backgroundLandscape.SetActive(!isPortrait);
        }
    }
}