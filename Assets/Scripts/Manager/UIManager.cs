using System;
using TMPro;
using UnityEngine;

namespace Hexfall.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject floatingScore;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI movesText;

        private ScoreManager scoreManager;

        private int currentMove = 0;

        public void Initialize()
        {
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
        }

        private void InitializeEvents()
        {
            EventManager.OnScoreChanged += scoreManager.IncreaseCurrentScore;
            EventManager.OnMoveChanged += IncreaseCurrentMove;
        }

        private void IncreaseCurrentMove()
        {
            currentMove++;
            movesText.text = currentMove.ToString();
        }
    }
}