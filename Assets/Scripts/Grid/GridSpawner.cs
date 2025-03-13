using Hexfall.Level;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridSpawner
    {
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private Transform hexagonParent;

        private int gridWidth, gridHeight;

        // for testing
        private ManualGrid manualGrid;

        public void Initialize(GridChecker gridChecker, LevelProperties levelProperties, HexagonProperties hexagonProperties, Transform hexagonParent)
        {
            this.hexagonProperties = hexagonProperties;
            this.hexagonParent = hexagonParent;

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
    }
}