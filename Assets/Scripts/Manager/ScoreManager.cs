using System.Collections.Generic;
using DG.Tweening;
using Hexfall.Hex;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Hexfall.Manager
{
    public class ScoreManager
    {
        private TextMeshProUGUI scoreText;
        private Transform floatingScorePrefab;

        private int currentScore = 0;

        public void Initialize(TextMeshProUGUI scoreText, Transform floatingScorePrefab)
        {
            this.scoreText = scoreText;
            this.floatingScorePrefab = floatingScorePrefab;
        }

        public void IncreaseCurrentScore(List<Hexagon> hexagons)
        {
            var hexCounter = 0;
            var bonusHexCounter = 0;
            var specialCounter = 0;

            var centerPointOfHexagons = Vector3.zero;

            foreach (var hexagon in hexagons)
            {
                // default hex 5 point
                // bonus hexagons are gives double point
                // specials are 10 point

                centerPointOfHexagons += hexagon.transform.position;

                if (hexagon.HexagonType == HexagonType.Default || hexagon.HexagonType == HexagonType.Bonus)
                {
                    hexCounter++;
                }

                if (hexagon.HexagonType == HexagonType.Bonus)
                {
                    bonusHexCounter++;
                }
                else if (hexagon.HexagonType == HexagonType.Special)
                {
                    specialCounter++;
                }
            }

            var bonusPoint = bonusHexCounter > 0 ? bonusHexCounter * 2 : 1;
            var comboScore = ((hexCounter * 5) + (specialCounter * 10)) * bonusPoint;
            currentScore += comboScore;

            scoreText.text = currentScore.ToString();

            // show the point of these hexagons at center 
            centerPointOfHexagons /= hexagons.Count;
            var newFloatingText = Object.Instantiate(floatingScorePrefab, centerPointOfHexagons, Quaternion.identity);
            var floatingText = newFloatingText.GetComponentInChildren<TextMeshPro>();
            floatingText.text = comboScore.ToString();
            Object.Destroy(newFloatingText.gameObject, 1f);
        }
    }
}