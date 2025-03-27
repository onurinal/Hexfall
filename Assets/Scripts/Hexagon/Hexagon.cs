using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.UIElements.Experimental;

namespace Hexfall.Hex
{
    public class Hexagon : MonoBehaviour
    {
        [SerializeField] private HexagonProperties hexagonProperties;
        [SerializeField] private SpriteRenderer hexSprite;
        [field: SerializeField] public HexagonType HexagonType { get; private set; }
        [field: SerializeField] public int IndexX { get; private set; }
        [field: SerializeField] public int IndexY { get; private set; }

        [SerializeField] private TextMeshProUGUI indexText;

        private Tween moveTween;
        private Tween rotateTween;
        private Tween destroyTween;
        private IEnumerator rotateCoroutine;

        private void Awake()
        {
            // to calculate scaleFactor and world positions of hex if the hexagon scale change
            UpdateScaleFactor();
        }

        private void OnDestroy()
        {
            moveTween?.Kill();
            destroyTween?.Kill();
            StopRotateCoroutine();
        }

        public void Initialize(int indexX, int indexY)
        {
            IndexX = indexX;
            IndexY = indexY;

            UpdateIndexText();

            HexagonType = (HexagonType)Random.Range(0, Enum.GetNames(typeof(HexagonType)).Length);

            switch (HexagonType)
            {
                case HexagonType.Red:
                    hexSprite.color = Color.red;
                    break;
                case HexagonType.Yellow:
                    hexSprite.color = Color.yellow;
                    break;
                case HexagonType.Blue:
                    hexSprite.color = Color.blue;
                    break;
                case HexagonType.Purple:
                    hexSprite.color = Color.magenta;
                    break;
                case HexagonType.Green:
                    hexSprite.color = Color.green;
                    break;
                case HexagonType.Cyan:
                    hexSprite.color = Color.cyan;
                    break;
            }
        }

        public void InitializeForTest(int indexX, int indexY, HexagonType hexagonType)
        {
            SetIndices(indexX, indexY);
            HexagonType = hexagonType;

            UpdateIndexText();

            switch (hexagonType)
            {
                case HexagonType.Red:
                    hexSprite.color = Color.red;
                    break;
                case HexagonType.Yellow:
                    hexSprite.color = Color.yellow;
                    break;
                case HexagonType.Blue:
                    hexSprite.color = Color.blue;
                    break;
                case HexagonType.Purple:
                    hexSprite.color = Color.magenta;
                    break;
                case HexagonType.Green:
                    hexSprite.color = Color.green;
                    break;
                case HexagonType.Cyan:
                    hexSprite.color = Color.cyan;
                    break;
            }
        }

        public void UpdateIndexText()
        {
            if (indexText != null)
            {
                indexText.transform.position = hexSprite.bounds.center;
                indexText.text = IndexX + "," + IndexY;
            }
        }

        public void Move(Vector2 targetPosition)
        {
            moveTween = transform.DOMove(targetPosition, hexagonProperties.MoveDuration).SetEase(Ease.InSine);
        }

        public void MoveWithNoDelay(Vector2 targetPosition)
        {
            transform.position = targetPosition;
        }

        private IEnumerator RotateCoroutine(Vector3 centerPosition, Vector2 targetPosition, float angle)
        {
            var timeElapsed = 0f;
            float initialAngle = transform.eulerAngles.z;
            float targetAngle = initialAngle + angle;

            hexSprite.sortingOrder++;
            while (timeElapsed < hexagonProperties.MoveDuration)
            {
                float currentAngle = Mathf.Lerp(initialAngle, targetAngle, timeElapsed / hexagonProperties.MoveDuration);
                transform.RotateAround(centerPosition, Vector3.forward, currentAngle - transform.eulerAngles.z);
                yield return null;

                timeElapsed += Time.deltaTime;
            }

            transform.RotateAround(centerPosition, Vector3.forward, targetAngle - transform.eulerAngles.z);
            transform.position = targetPosition;
            hexSprite.sortingOrder--;
            rotateCoroutine = null;
        }

        public void StartRotateCoroutine(Vector3 centerPosition, Vector2 targetPosition, float angle)
        {
            if (rotateCoroutine != null) return;

            rotateCoroutine = RotateCoroutine(centerPosition, targetPosition, angle);
            CoroutineHandler.Instance.StartCoroutine(rotateCoroutine);
        }

        private void StopRotateCoroutine()
        {
            if (rotateCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }

        private void UpdateScaleFactor()
        {
            var scaleFactorX = hexagonProperties.ScaleFactorX * GetComponentInChildren<Transform>().localScale.x;
            var scaleFactorY = hexagonProperties.ScaleFactorY * GetComponentInChildren<Transform>().localScale.y;
            hexagonProperties.SetScaleFactorX(scaleFactorX, scaleFactorY);
        }

        public void DestroyHexagon()
        {
            destroyTween = transform.DOScale(new Vector2(0f, 0f), hexagonProperties.DestroyDuration).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
        }

        public void HideHexagon()
        {
            gameObject.SetActive(false);
        }

        public void ShowHexagon()
        {
            gameObject.SetActive(true);
        }

        public void SetIndices(int indexX, int indexY)
        {
            IndexX = indexX;
            IndexY = indexY;

            UpdateIndexText();
        }
    }
}