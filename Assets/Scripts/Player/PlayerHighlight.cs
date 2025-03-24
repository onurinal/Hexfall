using System;
using Hexfall.Hex;
using Hexfall.Manager;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer groupHighlightSprite;
        // [SerializeField] private SpriteRenderer hexagonsCenterSprite;
        private PlayerMovement playerMovement;

        private readonly float xOffset = 0.092f;

        public void Initialize(PlayerMovement playerMovement)
        {
            this.playerMovement = playerMovement;

            EventManager.OnSwapping += HideHighlight;
            EventManager.OnSwapped += ShowHighlight;
        }

        private void OnDestroy()
        {
            EventManager.OnSwapping -= HideHighlight;
            EventManager.OnSwapped -= ShowHighlight;
        }

        public void DrawHexOutline(Hexagon firstHexagon, Hexagon secondHexagon, Hexagon thirdHexagon)
        {
            if (groupHighlightSprite == null) return;

            float angle = Mathf.Round(playerMovement.GetAngle(firstHexagon.transform.position, secondHexagon.transform.position));
            var centerPositionOfVectors = (firstHexagon.transform.position + secondHexagon.transform.position + thirdHexagon.transform.position) / 3f;
            var newCenterX = GetNewXPosition(angle, centerPositionOfVectors.x);
            centerPositionOfVectors = new Vector3(newCenterX, centerPositionOfVectors.y, centerPositionOfVectors.z);

            groupHighlightSprite.transform.position = centerPositionOfVectors;
            groupHighlightSprite.enabled = true;
        }

        private float GetNewXPosition(float angle, float centerPosX)
        {
            switch ((int)angle)
            {
                case 0:
                    groupHighlightSprite.flipX = true;
                    return centerPosX + xOffset;
                case 60:
                    groupHighlightSprite.flipX = false;
                    return centerPosX - xOffset;
                case 120:
                    groupHighlightSprite.flipX = true;
                    return centerPosX + xOffset;
                case 180:
                    groupHighlightSprite.flipX = false;
                    return centerPosX - xOffset;
                case 240:
                    groupHighlightSprite.flipX = true;
                    return centerPosX + xOffset;
                case 300:
                    groupHighlightSprite.flipX = false;
                    return centerPosX - xOffset;
                default:
                    groupHighlightSprite.flipX = false;
                    return centerPosX + xOffset;
            }
        }

        private void HideHighlight()
        {
            groupHighlightSprite.enabled = false;
        }

        private void ShowHighlight()
        {
            groupHighlightSprite.enabled = true;
        }
    }
}