using System;
using TMPro;
using UnityEngine;

namespace Hexfall.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI movesText;

        private ScoreManager scoreManager;

        public void Initialize()
        {
            scoreManager = new ScoreManager();
            scoreManager.Initialize(scoreText);

            InitializeEvents();
        }

        private void OnDestroy()
        {
            if (scoreManager != null)
            {
                EventManager.OnScoreChanged -= scoreManager.UpdateScore;
            }
        }

        private void InitializeEvents()
        {
            EventManager.OnScoreChanged += scoreManager.UpdateScore;
        }
    }
}