using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.CameraManager
{
    public class CameraController : MonoBehaviour
    {
        public void Initialize(LevelProperties levelProperties, HexagonProperties hexagonProperties)
        {
            UpdateCameraPosition(levelProperties, hexagonProperties);
        }

        private void UpdateCameraPosition(LevelProperties levelProperties, HexagonProperties hexagonProperties)
        {
            var gridWidth = levelProperties.GridWidth;
            var gridHeight = levelProperties.GridHeight;
            var scaleFactorX = hexagonProperties.ScaleFactorX;
            var scaleFactorY = hexagonProperties.ScaleFactorY;

            var newPositionX = (gridWidth / 2f * scaleFactorX) - (scaleFactorX / 2f);
            var newPositionY = gridHeight * scaleFactorY;

            transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
        }
    }
}