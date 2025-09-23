using System.Collections;
using Hexfall.CameraManager;
using Hexfall.Level;
using Hexfall.Hex;
using Hexfall.Manager;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridSpawner
    {
        private LevelManager levelManager;
        private CameraController cameraController;
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private Transform hexagonParent;

        private int gridWidth, gridHeight;

        private IEnumerator createNewHexagonToEmptySlotCoroutine;

        // for testing
        private ManualGrid manualGrid;

        public void Initialize(LevelManager levelManager, GridChecker gridChecker, GridMovement gridMovement, LevelProperties levelProperties, HexagonProperties hexagonProperties,
        Transform hexagonParent,
        CameraController cameraController)
        {
            this.levelManager = levelManager;
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

            gridChecker.Initialize(hexagonGrid, levelManager, levelProperties);
            gridMovement.Initialize(hexagonGrid, this, levelManager, gridChecker, levelProperties, hexagonProperties, cameraController);
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

        public Hexagon GetHexagonAtAxis(int indexX, int indexY)
        {
            if (indexX < 0 || indexY < 0 || indexY >= gridHeight || indexX >= gridWidth) return null;
            if (hexagonGrid[indexX, indexY] == null) return null;

            return hexagonGrid[indexX, indexY];
        }

        private Hexagon CreateNewHexagon(int width, int height, Vector2 position)
        {
            var hexagonType = TryToGetOtherThanDefaultHexagon();
            Hexagon hexagon = hexagonType == HexagonType.Special ? hexagonProperties.BombHexagonPrefab : hexagonProperties.DefaultHexagonPrefab;

            var newHex = Object.Instantiate(hexagon, position, Quaternion.identity, hexagonParent);
            newHex.Initialize(width, height, hexagonType);
            return newHex;
        }

        private HexagonType TryToGetOtherThanDefaultHexagon()
        {
            if (levelManager.IsGridInitializing) return HexagonType.Default;

            var randomPercentage = Random.Range(0, 101);

            if (randomPercentage >= 0 && randomPercentage <= hexagonProperties.DefaultHexPossibility)
            {
                return HexagonType.Default;
            }

            if (randomPercentage > hexagonProperties.DefaultHexPossibility && randomPercentage <= hexagonProperties.DefaultHexPossibility + hexagonProperties.BonusHexPossibility)
            {
                return HexagonType.Bonus;
            }

            if (randomPercentage > hexagonProperties.DefaultHexPossibility + hexagonProperties.BonusHexPossibility &&
                randomPercentage <= hexagonProperties.DefaultHexPossibility + hexagonProperties.BonusHexPossibility + hexagonProperties.SpecialHexPossibility)
            {
                return HexagonType.Special;
            }

            return HexagonType.Default;
        }

        private IEnumerator CreateNewHexagonToEmptySlotCoroutine(float moveDuration)
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
                        if (levelManager.IsGridInitializing)
                        {
                            hexagonGrid[width, height].HideHexagon();
                        }

                        var newTargetPosition = GetHexagonWorldPosition(width, height);
                        hexagonGrid[width, height].Move(newTargetPosition, moveDuration);
                    }
                }

                yield return new WaitForSeconds(moveDuration / 3f);
            }

            yield return new WaitForSeconds(moveDuration - (moveDuration / 3f));
            createNewHexagonToEmptySlotCoroutine = null;
        }

        public IEnumerator StartCreateNewHexagonToEmptySlot(float moveDuration)
        {
            if (createNewHexagonToEmptySlotCoroutine != null) yield break;

            createNewHexagonToEmptySlotCoroutine = CreateNewHexagonToEmptySlotCoroutine(moveDuration);
            yield return createNewHexagonToEmptySlotCoroutine;
        }

        private void StopCreateNewHexagonToEmptySlot()
        {
            if (createNewHexagonToEmptySlotCoroutine != null)
            {
                CoroutineHandler.Instance.StopCoroutine(createNewHexagonToEmptySlotCoroutine);
                createNewHexagonToEmptySlotCoroutine = null;
            }
        }

        public void ShowAllHexagons()
        {
            foreach (var hexagon in hexagonGrid)
            {
                hexagon.ShowHexagon();
            }
        }

        public void HideAllHexagons()
        {
            foreach (var hexagon in hexagonGrid)
            {
                hexagon.HideHexagon();
            }
        }
    }
}