using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
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

        private void Awake()
        {
            // to calculate scaleFactor and world positions of hex if the hexagon scale change
            UpdateScaleFactor();
        }

        private void OnDestroy()
        {
            moveTween?.Kill();
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
            indexText.transform.position = hexSprite.bounds.center;
            indexText.text = IndexX + "," + IndexY;
        }

        public void Move(Vector2 targetPosition)
        {
            transform.DOMove(targetPosition, hexagonProperties.MoveDuration).SetEase(Ease.InSine);
        }

        public void MoveWithNoDelay(Vector2 targetPosition)
        {
            transform.position = targetPosition;
        }

        private void UpdateScaleFactor()
        {
            var scaleFactorX = hexagonProperties.ScaleFactorX * GetComponentInChildren<Transform>().localScale.x;
            var scaleFactorY = hexagonProperties.ScaleFactorY * GetComponentInChildren<Transform>().localScale.y;
            hexagonProperties.SetScaleFactorX(scaleFactorX, scaleFactorY);
        }

        public void DestroyHexagon()
        {
            transform.DOScale(new Vector2(0f, 0f), hexagonProperties.DestroyDuration).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));
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
        }
    }
}