using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerHighlight
    {
        private PlayerSelection playerSelection;
        private SpriteRenderer groupHighlightSprite;

        private readonly float highlightScaleFactorX = 0.275f; // for group highlight sprite
        private readonly float highlightScaleFactorY = 0.321f;

        public void Initialize(PlayerSelection playerSelection, SpriteRenderer groupHighlightSprite)
        {
            this.playerSelection = playerSelection;
            this.groupHighlightSprite = groupHighlightSprite;
        }

        public void DrawHexOutline(Hexagon selectedHexagon, Hexagon second)
        {
            if (groupHighlightSprite == null) return;

            float angle = Mathf.Round(playerSelection.GetAngle(selectedHexagon.transform.position, second.transform.position));
            groupHighlightSprite.transform.position = GetHexagonOutlineWorldPosition(selectedHexagon.IndexX, selectedHexagon.IndexY, angle);
            groupHighlightSprite.enabled = true;
        }

        private Vector2 GetHexagonOutlineWorldPosition(int width, int height, float angle)
        {
            var newWidth = width;
            var newHeight = height;
            var flipX = false;
            float yPos;

            switch ((int)angle)
            {
                case 0:
                    yPos = (height * 2f * highlightScaleFactorY) + (width % 2 == 0 ? highlightScaleFactorY * 2 : highlightScaleFactorY);
                    flipX = true;
                    break;

                case 60:
                    yPos = (height * 2f * highlightScaleFactorY) + ((width % 2 == 0) ? highlightScaleFactorY : 0);
                    break;

                case 120:
                    newHeight -= 1;
                    yPos = (newHeight * 2f * highlightScaleFactorY) + ((width % 2 == 0) ? highlightScaleFactorY * 2 : highlightScaleFactorY);
                    flipX = true;
                    break;

                case 180:
                    newWidth -= 1;
                    newHeight = (width % 2 == 0) ? height : height - 1;
                    yPos = (newHeight * 2f * highlightScaleFactorY) + ((width % 2 == 0) ? 0 : highlightScaleFactorY);
                    break;

                case 240:
                    newWidth -= 1;
                    yPos = (height * 2f * highlightScaleFactorY) + ((width % 2 == 0) ? highlightScaleFactorY : 0);
                    flipX = true;
                    break;

                case 300:
                    newWidth -= 1;
                    newHeight = (width % 2 == 0) ? height + 1 : height;
                    yPos = (newHeight * 2f * highlightScaleFactorY) + ((width % 2 == 0) ? 0 : highlightScaleFactorY);
                    break;

                default:
                    return Vector2.zero;
            }

            float xPos = (newWidth * highlightScaleFactorX * 2f) + highlightScaleFactorX;
            groupHighlightSprite.flipX = flipX;
            return new Vector2(xPos, yPos);
        }
    }
}