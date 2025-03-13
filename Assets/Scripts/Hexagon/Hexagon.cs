using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private void Awake()
        {
            // to calculate scaleFactor and world positions of hex if the hexagon scale change
            UpdateScaleFactor();
        }

        public void Initialize(int indexX, int indexY)
        {
            IndexX = indexX;
            IndexY = indexY;

            indexText.transform.position = hexSprite.bounds.center;
            indexText.text = IndexX + "," + IndexY;

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
            IndexX = indexX;
            IndexY = indexY;
            HexagonType = hexagonType;

            indexText.transform.position = hexSprite.bounds.center;
            indexText.text = IndexX + "," + IndexY;

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

        private void UpdateScaleFactor()
        {
            var scaleFactorX = hexagonProperties.ScaleFactorX * GetComponentInChildren<Transform>().localScale.x;
            var scaleFactorY = hexagonProperties.ScaleFactorY * GetComponentInChildren<Transform>().localScale.y;
            hexagonProperties.SetScaleFactorX(scaleFactorX, scaleFactorY);
        }

        public void DestroyHexagon()
        {
            Destroy(gameObject);
        }
    }
}