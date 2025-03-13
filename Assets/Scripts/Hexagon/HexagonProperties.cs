using UnityEngine;

namespace Hexfall.Hex
{
    [CreateAssetMenu(fileName = "Hexagon Properties", menuName = "Hexfall/Create New HexagonProperties")]
    public class HexagonProperties : ScriptableObject
    {
        [SerializeField] private Hexagon hexagonPrefab;

        public Hexagon HexagonPrefab => hexagonPrefab;

        [field: Tooltip("To calculate world position of the hexagon")]
        [field: SerializeField] public float ScaleFactorX { get; private set; } = 0.475f;
        [field: SerializeField] public float ScaleFactorY { get; private set; } = 0.275f;

        public void SetScaleFactorX(float scaleFactorX, float scaleFactorY)
        {
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
        }
    }
}