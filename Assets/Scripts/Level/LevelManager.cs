using Hexfall.CameraManager;
using Hexfall.Grid;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private LevelProperties levelProperties;

        private GridSpawner gridSpawner;

        public void Initialize(HexagonProperties hexagonProperties, Transform hexagonParent)
        {
            cameraController.Initialize(levelProperties, hexagonProperties);
            gridSpawner = new GridSpawner();
            gridSpawner.Initialize(levelProperties, hexagonProperties, hexagonParent);
        }
    }
}