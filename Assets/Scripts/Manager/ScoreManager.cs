using System.Collections.Generic;
using Hexfall.Hex;
using TMPro;

namespace Hexfall.Manager
{
    public class ScoreManager
    {
        private TextMeshProUGUI scoreText;

        private int currentScore = 0;

        public void Initialize(TextMeshProUGUI scoreText)
        {
            this.scoreText = scoreText;
        }

        public void UpdateScore(List<Hexagon> hexagons)
        {
            var defaultHexCounter = 0;
            var bonusHexCounter = 0;
            var specialCounter = 0;

            foreach (var hexagon in hexagons)
            {
                // default hex 5 point
                // bonus hexagons are gives double point
                // specials are 10 point

                if (hexagon.HexagonType == HexagonType.Default)
                {
                    defaultHexCounter++;
                }
                else if (hexagon.HexagonType == HexagonType.Bonus)
                {
                    bonusHexCounter++;
                }
                else if (hexagon.HexagonType == HexagonType.Special)
                {
                    specialCounter++;
                }
            }

            currentScore += ((defaultHexCounter * 5) + (specialCounter * 10)) + (2 * bonusHexCounter * ((defaultHexCounter * 5) + (specialCounter * 10)));

            scoreText.text = currentScore.ToString();
        }
    }
}