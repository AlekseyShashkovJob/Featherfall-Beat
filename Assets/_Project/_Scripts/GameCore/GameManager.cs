using TMPro;
using UnityEngine;

namespace GameCore
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int CurrentScore { get; private set; } = 0;
        public int BestScore { get; private set; } = 0;
        public bool IsGameActive { get; private set; } = true;

        public GameDifficulty CurrentDifficulty { get; private set; } = GameDifficulty.Easy;

        [SerializeField] private View.UI.UIScreen _difficultyScreen;
        [SerializeField] private View.UI.UIScreen _tutorialScreen;
        [SerializeField] private View.UI.UIScreen _winScreen;
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;
        [SerializeField] private TMP_Text _scoreText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Time.timeScale = 1.0f;

            IsGameActive = false;
            CurrentScore = 0;
            UpdateScoreUI();

            _difficultyScreen.StartScreen();
        }

        public void SetDifficulty(int difficultyIndex)
        {
            CurrentDifficulty = (GameDifficulty)difficultyIndex;

            _difficultyScreen.CloseScreen();

            _tutorialScreen.StartScreen();
        }

        public void StartGameFromTutorial()
        {
            _tutorialScreen.CloseScreen();

            IsGameActive = true;

            var tileManager = FindObjectOfType<GameCore.Objects.TileManager>();
            if (tileManager != null)
                tileManager.OnGameStart();
        }

        public float GetSpawnIntervalMultiplier()
        {
            return CurrentDifficulty switch
            {
                GameDifficulty.Easy => 1.0f,
                GameDifficulty.Medium => 1.5f,
                GameDifficulty.Hard => 2.0f,
                _ => 1.0f
            };
        }

        public void AddPoints(int amount = 1)
        {
            if (!IsGameActive) return;

            Misc.Services.VibroManager.Vibrate();

            CurrentScore += amount;
            UpdateScoreUI();
        }

        public void RestartGame()
        {
            IsGameActive = false;
            Time.timeScale = 1.0f;
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.GAME_SCENE);
        }

        public void FinishGame()
        {
            if (!IsGameActive) return;

            Misc.Services.VibroManager.Vibrate();

            IsGameActive = false;
            Time.timeScale = 0.0f;

            if (CurrentScore > BestScore)
                BestScore = CurrentScore;

            SaveData();

            _winScreen.StartScreen();
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            _scoreText.text = $"{CurrentScore}";
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(GameConstants.BEST_SCORE_KEY, BestScore);
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            BestScore = PlayerPrefs.GetInt(GameConstants.BEST_SCORE_KEY, 0);
        }
    }
}