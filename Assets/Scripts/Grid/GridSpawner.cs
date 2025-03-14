using System.Collections;
using DefaultNamespace;
using Hexfall.CameraManager;
using Hexfall.Level;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridSpawner
    {
        private CameraController cameraController;
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private Transform hexagonParent;

        private int gridWidth, gridHeight;

        private IEnumerator createNewHexagonToEmptySlotCoroutine;

        // for testing
        private ManualGrid manualGrid;

        public void Initialize(GridChecker gridChecker, GridMovement gridMovement, LevelProperties levelProperties, HexagonProperties hexagonProperties, Transform hexagonParent, CameraController cameraController)
        {
            this.hexagonProperties = hexagonProperties;
            this.hexagonParent = hexagonParent;
            this.cameraController = cameraController;

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;

            CreateNewGrid();

            // for testing
            manualGrid = new ManualGrid();
            if (manualGrid.IsTesting)
            {
                manualGrid.Initialize(hexagonGrid, hexagonProperties, this);
            }
            else
            {
                CreateHexagonsToNewGrid();
            }

            gridChecker.Initialize(hexagonGrid, levelProperties);
            gridMovement.Initialize(hexagonGrid, this, levelProperties, hexagonProperties);
        }

        private void CreateNewGrid()
        {
            hexagonGrid = new Hexagon[gridWidth, gridHeight];
        }

        private void CreateHexagonsToNewGrid()
        {
            for (int height = 0; height < gridHeight; height++)
            {
                for (int width = 0; width < gridWidth; width++)
                {
                    var hexPosition = GetHexagonWorldPosition(width, height);
                    hexagonGrid[width, height] = CreateNewHexagon(width, height, hexPosition);
                }
            }
        }

        public Vector2 GetHexagonWorldPosition(int width, int height)
        {
            var positionX = (hexagonProperties.ScaleFactorX * width);
            var positionY = width % 2 == 0 ? (hexagonProperties.ScaleFactorY * 2 * height) + hexagonProperties.ScaleFactorY : hexagonProperties.ScaleFactorY * 2 * height;
            return new Vector2(positionX, positionY);
        }

        private Hexagon CreateNewHexagon(int width, int height, Vector2 position)
        {
            var hexagon = Object.Instantiate(hexagonProperties.HexagonPrefab, position, Quaternion.identity, hexagonParent);
            hexagon.Initialize(width, height);
            return hexagon;
        }

        private IEnumerator CreateNewHexagonToEmptySlotCoroutine()
        {
            for (int width = 0; width < gridWidth; width++)
            {
                for (int height = 0; height < gridHeight; height++)
                {
                    if (hexagonGrid[width, height] == null)
                    {
                        // first targetPosition is to spawn hexagon at the top of the screen
                        // new targetPosition is to move hexagon in empty slots
                        var targetPositionX = GetHexagonWorldPosition(width, height).x;
                        var targetPositionY = cameraController.GetTopLeftWorldPosition().y;
                        hexagonGrid[width, height] = CreateNewHexagon(width, height, new Vector2(targetPositionX, targetPositionY));
                        var newTargetPosition = GetHexagonWorldPosition(width, height);
                        hexagonGrid[width, height].Move(newTargetPosition);
                    }
                }

                yield return new WaitForSeconds(hexagonProperties.MoveDuration / 3f);
            }

            createNewHexagonToEmptySlotCoroutine = null;
        }

        public IEnumerator StartCreateNewHexagonToEmptySlot()
        {
            if (createNewHexagonToEmptySlotCoroutine != null) yield break;

            createNewHexagonToEmptySlotCoroutine = CreateNewHexagonToEmptySlotCoroutine();
            CoroutineHandler.Instance.StartCoroutine(createNewHexagonToEmptySlotCoroutine);
        }

        private void StopCreateNewHexagonToEmptySlot()
        {
            if (createNewHexagonToEmptySlotCoroutine != null)
            {
                CoroutineHandler.Instance.StopCoroutine(createNewHexagonToEmptySlotCoroutine);
                createNewHexagonToEmptySlotCoroutine = null;
            }
        }
    }
}