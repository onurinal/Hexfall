using System.Collections;
using TMPro;
using UnityEngine;

namespace Hexfall.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform loseUI;
        [SerializeField] private Transform floatingScore;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI movesText;

        private ScoreManager scoreManager;
        private float destroyDuration;
        private IEnumerator gameOverCoroutine;

        private int currentMove = 0;

        public void Initialize(float destroyDuration)
        {
            this.destroyDuration = destroyDuration;
            scoreManager = new ScoreManager();
            scoreManager.Initialize(scoreText, floatingScore);

            InitializeEvents();
        }

        private void OnDestroy()
        {
            if (scoreManager != null)
            {
                EventManager.OnScoreChanged -= scoreManager.IncreaseCurrentScore;
            }

            EventManager.OnMoveChanged -= IncreaseCurrentMove;
            EventManager.OnGameOver -= StartGameOverCoroutine;
        }

        private void InitializeEvents()
        {
            EventManager.OnScoreChanged += scoreManager.IncreaseCurrentScore;
            EventManager.OnMoveChanged += IncreaseCurrentMove;
            EventManager.OnGameOver += StartGameOverCoroutine;
        }

        private void IncreaseCurrentMove()
        {
            currentMove++;
            movesText.text = currentMove.ToString();
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(destroyDuration);
            loseUI.gameObject.SetActive(true);
        }

        private void StartGameOverCoroutine()
        {
            if (gameOverCoroutine != null) return;

            gameOverCoroutine = GameOverCoroutine();
            CoroutineHandler.Instance.StartCoroutine(gameOverCoroutine);
        }

        private void StopGameOverCoroutine()
        {
            if (gameOverCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(gameOverCoroutine);
            gameOverCoroutine = null;
        }
    }
}