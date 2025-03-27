using System;
using System.Collections;
using Hexfall.Hex;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Hexfall.Player
{
    public class PlayerHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer groupHighlightSprite;
        [SerializeField] private SpriteRenderer hexagonsCenterSprite;
        private PlayerMovement playerMovement;
        private HexagonProperties hexagonProperties;
        private IEnumerator rotateCoroutine;

        // to reset position if highlight angle changes
        private Vector3 originalPosition;

        private readonly float xOffset = 0.08f;

        public void Initialize(PlayerMovement playerMovement, HexagonProperties hexagonProperties)
        {
            this.playerMovement = playerMovement;
            this.hexagonProperties = hexagonProperties;
        }

        private void OnDestroy()
        {
            StopRotateCoroutine();
        }

        public void DrawHexOutline(Hexagon firstHexagon, Hexagon secondHexagon, Hexagon thirdHexagon)
        {
            if (groupHighlightSprite == null) return;

            var centerPositionOfVectors = (firstHexagon.transform.position + secondHexagon.transform.position + thirdHexagon.transform.position) / 3f;
            hexagonsCenterSprite.transform.position = centerPositionOfVectors;

            // for group highlighter, needs to add some offset because of sprite size
            var angle = Mathf.Round(playerMovement.GetAngle(firstHexagon.transform.position, secondHexagon.transform.position));

            var newGroupHighlightX = GetNewXPosition(centerPositionOfVectors.x, (int)angle);
            originalPosition = new Vector3(newGroupHighlightX, centerPositionOfVectors.y, centerPositionOfVectors.z);
            groupHighlightSprite.transform.position = originalPosition;

            ShowHighlight();
        }

        private float GetNewXPosition(float centerPosX, int angle)
        {
            var shouldFlip = angle == 0 || angle == 120 || angle == 240;
            groupHighlightSprite.flipX = shouldFlip;
            return centerPosX + (shouldFlip ? xOffset : -xOffset);
        }

        private bool IsAngleChanged()
        {
            var zAngle = groupHighlightSprite.transform.eulerAngles.z % 360;
            return Mathf.Abs(zAngle) > 0.1f;
        }

        private void ResetAngle()
        {
            if (groupHighlightSprite == null || !IsAngleChanged()) return;

            groupHighlightSprite.transform.rotation = Quaternion.identity;
        }

        private void ResetToOriginalPosition()
        {
            if (groupHighlightSprite == null) return;

            if (groupHighlightSprite.transform.position != originalPosition)
            {
                groupHighlightSprite.transform.position = originalPosition;
            }
        }

        public void HideHighlight()
        {
            if (hexagonsCenterSprite == null || groupHighlightSprite == null) return;

            hexagonsCenterSprite.gameObject.SetActive(false);
            groupHighlightSprite.gameObject.SetActive(false);

            if (IsAngleChanged())
            {
                ResetAngle();
            }
        }

        public void ShowHighlight()
        {
            if (hexagonsCenterSprite == null || groupHighlightSprite == null) return;

            ResetToOriginalPosition();
            groupHighlightSprite.gameObject.SetActive(true);
            hexagonsCenterSprite.gameObject.SetActive(true);
        }

        private IEnumerator RotateCoroutine(Vector3 centerPosition, float angle)
        {
            float timeElapsed = 0f;
            var initialAngle = groupHighlightSprite.transform.eulerAngles.z;
            var targetAngle = angle + initialAngle;

            while (timeElapsed < hexagonProperties.MoveDuration)
            {
                var currentAngle = Mathf.Lerp(initialAngle, targetAngle, timeElapsed / hexagonProperties.MoveDuration);
                groupHighlightSprite.transform.RotateAround(centerPosition, Vector3.forward, currentAngle - groupHighlightSprite.transform.eulerAngles.z);
                yield return null;

                timeElapsed += Time.deltaTime;
            }

            groupHighlightSprite.transform.RotateAround(centerPosition, Vector3.forward, targetAngle - groupHighlightSprite.transform.eulerAngles.z);
            rotateCoroutine = null;
        }

        public void StartRotateCoroutine(Vector3 centerPosition, float angle)
        {
            if (rotateCoroutine != null) return;

            rotateCoroutine = RotateCoroutine(centerPosition, angle);
            CoroutineHandler.Instance.StartCoroutine(rotateCoroutine);
        }

        private void StopRotateCoroutine()
        {
            if (rotateCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }
}