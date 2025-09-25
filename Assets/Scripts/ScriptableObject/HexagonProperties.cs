using UnityEngine;

namespace Hexfall.Hex
{
    [CreateAssetMenu(fileName = "Hexagon Properties", menuName = "Hexfall/Create New HexagonProperties")]
    public class HexagonProperties : ScriptableObject
    {
        [Header("Hexagon Type")]
        [SerializeField] private Hexagon defaultHexagonPrefab;
        [SerializeField] private Hexagon bombHexagonPrefab;

        [Header("Hexagon Duration")]
        [SerializeField] private float moveDuration;
        [SerializeField] private float destroyDuration;

        [Header("Possibilities")]
        [SerializeField] private int defaultHexPossibility;
        [SerializeField] private int bonusHexPossibility = 10;
        [SerializeField] private int specialHexPossibility = 10;

        [Header("Hexagon Rotation")]
        [SerializeField] private float rotationAngle = 120f;

        [field: Tooltip("To calculate world position of the hexagon")]
        [field: Header("Other Properties")]
        [field: SerializeField] public float ScaleFactorX { get; private set; } = 0.48f;
        [field: SerializeField] public float ScaleFactorY { get; private set; } = 0.32f;

        public Hexagon DefaultHexagonPrefab => defaultHexagonPrefab;
        public Hexagon BombHexagonPrefab => bombHexagonPrefab;
        public float MoveDuration => moveDuration;
        public float DestroyDuration => destroyDuration;
        public float RotationAngle => rotationAngle;
        public int DefaultHexPossibility => defaultHexPossibility;
        public int BonusHexPossibility => bonusHexPossibility;
        public int SpecialHexPossibility => specialHexPossibility;

        public void SetScaleFactorX(float scaleFactorX, float scaleFactorY)
        {
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
        }
    }
}