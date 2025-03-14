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

        // to find world position of top left of the screen
        public Vector3 GetTopLeftWorldPosition()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("No main camera found.");
                return Vector3.zero;
            }

            var topLeftPosition = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, transform.position.z));
            return topLeftPosition;
        }
    }
}