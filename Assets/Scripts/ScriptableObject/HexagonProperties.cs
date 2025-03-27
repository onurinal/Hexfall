using UnityEngine;

namespace Hexfall.Hex
{
    [CreateAssetMenu(fileName = "Hexagon Properties", menuName = "Hexfall/Create New HexagonProperties")]
    public class HexagonProperties : ScriptableObject
    {
        [Header("Hexagon Type")]
        [SerializeField] private Hexagon defaultHexagon;

        [Header("Hexagon Duration")]
        [SerializeField] private float moveDuration;
        [SerializeField] private float destroyDuration;

        [Header("Hexagon Rotation")]
        [SerializeField] private float rotationAngle = 120f;

        [field: Tooltip("To calculate world position of the hexagon")]
        [field: Header("Other Properties")]
        [field: SerializeField] public float ScaleFactorX { get; private set; } = 0.48f;
        [field: SerializeField] public float ScaleFactorY { get; private set; } = 0.32f;

        public Hexagon DefaultHexagon => defaultHexagon;
        public float MoveDuration => moveDuration;
        public float DestroyDuration => destroyDuration;
        public float RotationAngle => rotationAngle;

        public void SetScaleFactorX(float scaleFactorX, float scaleFactorY)
        {
            ScaleFactorX = scaleFactorX;
            ScaleFactorY = scaleFactorY;
        }
    }
}