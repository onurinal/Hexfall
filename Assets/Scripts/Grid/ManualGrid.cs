using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Grid
{
    public class ManualGrid
    {
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private GridSpawner gridSpawner;

        public readonly bool IsTesting = false;

        public void Initialize(Hexagon[,] hexagonGrid, HexagonProperties hexagonProperties, GridSpawner gridSpawner)
        {
            this.hexagonGrid = hexagonGrid;
            this.hexagonProperties = hexagonProperties;
            this.gridSpawner = gridSpawner;

            CreateNewHexagonsToGrid();
        }

        private void CreateNewHexagonsToGrid()
        {
            hexagonGrid[0, 0] = CreateNewHexagon(0, 0, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 0] = CreateNewHexagon(1, 0, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 0] = CreateNewHexagon(2, 0, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 0] = CreateNewHexagon(3, 0, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 0] = CreateNewHexagon(4, 0, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 0] = CreateNewHexagon(5, 0, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 0] = CreateNewHexagon(6, 0, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 0] = CreateNewHexagon(7, 0, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 1] = CreateNewHexagon(0, 1, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 1] = CreateNewHexagon(1, 1, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 1] = CreateNewHexagon(2, 1, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 1] = CreateNewHexagon(3, 1, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 1] = CreateNewHexagon(4, 1, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 1] = CreateNewHexagon(5, 1, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 1] = CreateNewHexagon(6, 1, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 1] = CreateNewHexagon(7, 1, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 2] = CreateNewHexagon(0, 2, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 2] = CreateNewHexagon(1, 2, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 2] = CreateNewHexagon(2, 2, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 2] = CreateNewHexagon(3, 2, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 2] = CreateNewHexagon(4, 2, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 2] = CreateNewHexagon(5, 2, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 2] = CreateNewHexagon(6, 2, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 2] = CreateNewHexagon(7, 2, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 3] = CreateNewHexagon(0, 3, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 3] = CreateNewHexagon(1, 3, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 3] = CreateNewHexagon(2, 3, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 3] = CreateNewHexagon(3, 3, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 3] = CreateNewHexagon(4, 3, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 3] = CreateNewHexagon(5, 3, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 3] = CreateNewHexagon(6, 3, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Bonus);
            hexagonGrid[7, 3] = CreateNewHexagon(7, 3, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 4] = CreateNewHexagon(0, 4, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 4] = CreateNewHexagon(1, 4, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 4] = CreateNewHexagon(2, 4, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 4] = CreateNewHexagon(3, 4, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 4] = CreateNewHexagon(4, 4, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 4] = CreateNewHexagon(5, 4, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 4] = CreateNewHexagon(6, 4, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 4] = CreateNewHexagon(7, 4, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 5] = CreateNewHexagon(0, 5, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 5] = CreateNewHexagon(1, 5, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 5] = CreateNewHexagon(2, 5, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 5] = CreateNewHexagon(3, 5, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 5] = CreateNewHexagon(4, 5, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 5] = CreateNewHexagon(5, 5, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 5] = CreateNewHexagon(6, 5, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 5] = CreateNewHexagon(7, 5, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 6] = CreateNewHexagon(0, 6, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 6] = CreateNewHexagon(1, 6, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 6] = CreateNewHexagon(2, 6, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 6] = CreateNewHexagon(3, 6, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 6] = CreateNewHexagon(4, 6, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 6] = CreateNewHexagon(5, 6, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 6] = CreateNewHexagon(6, 6, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 6] = CreateNewHexagon(7, 6, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 7] = CreateNewHexagon(0, 7, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 7] = CreateNewHexagon(1, 7, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 7] = CreateNewHexagon(2, 7, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 7] = CreateNewHexagon(3, 7, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 7] = CreateNewHexagon(4, 7, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 7] = CreateNewHexagon(5, 7, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 7] = CreateNewHexagon(6, 7, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 7] = CreateNewHexagon(7, 7, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);

            hexagonGrid[0, 8] = CreateNewHexagon(0, 8, HexagonColorType.Yellow, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[1, 8] = CreateNewHexagon(1, 8, HexagonColorType.Purple, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[2, 8] = CreateNewHexagon(2, 8, HexagonColorType.Green, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[3, 8] = CreateNewHexagon(3, 8, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[4, 8] = CreateNewHexagon(4, 8, HexagonColorType.Red, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[5, 8] = CreateNewHexagon(5, 8, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[6, 8] = CreateNewHexagon(6, 8, HexagonColorType.Blue, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
            hexagonGrid[7, 8] = CreateNewHexagon(7, 8, HexagonColorType.Cyan, hexagonProperties.DefaultHexagonPrefab, HexagonType.Default);
        }

        private Hexagon CreateNewHexagon(int width, int height, HexagonColorType hexagonColorType, Hexagon hexagonPrefab, HexagonType hexagonType)
        {
            var hexPosition = gridSpawner.GetHexagonWorldPosition(width, height);
            var hexagon = Object.Instantiate(hexagonPrefab, hexPosition, Quaternion.identity);
            hexagon.InitializeForTest(width, height, hexagonColorType, hexagonType);
            return hexagon;
        }
    }
}