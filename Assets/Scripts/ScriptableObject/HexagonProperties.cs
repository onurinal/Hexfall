using UnityEngine;

namespace Hexfall.Hex
{
    [CreateAssetMenu(fileName = "Hexagon Properties", menuName = "Hexfall/Create New HexagonProperties")]
    public class HexagonProperties : ScriptableObject
    {
        [SerializeField] private Hexagon hexagonPrefab;

        [Header("Hexagon Durations")]
        [SerializeField] private float moveDuration;
        [SerializeField] private float destroyDuration;

        [field: Tooltip("To calculate world position of the hexagon")]
        [field: SerializeField] public float ScaleFactorX { get; private set; } = 0.48f;
        [field: SerializeField] public float ScaleFactorY { get; private set; } = 0.32f;

        public Hexagon HexagonPrefab => hexagonPrefab;
        public float MoveDuration => moveDuration;
        public float DestroyDuration => destroyDuration;

        public void SetScaleFactorX(float scaleFactorX, float scaleFactorY)
        {
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
        }
    }
}