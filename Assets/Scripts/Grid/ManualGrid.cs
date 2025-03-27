using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Grid
{
    public class ManualGrid
    {
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private GridSpawner gridSpawner;

        public readonly bool IsTesting = true;

        public void Initialize(Hexagon[,] hexagonGrid, HexagonProperties hexagonProperties, GridSpawner gridSpawner)
        {
            this.hexagonGrid = hexagonGrid;
            this.hexagonProperties = hexagonProperties;
            this.gridSpawner = gridSpawner;

            CreateNewHexagonsToGrid();
        }

        private void CreateNewHexagonsToGrid()
        {
            hexagonGrid[0, 0] = CreateNewHexagon(0, 0, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 0] = CreateNewHexagon(1, 0, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 0] = CreateNewHexagon(2, 0, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 0] = CreateNewHexagon(3, 0, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 0] = CreateNewHexagon(4, 0, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 0] = CreateNewHexagon(5, 0, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 0] = CreateNewHexagon(6, 0, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 0] = CreateNewHexagon(7, 0, HexagonType.Cyan, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 1] = CreateNewHexagon(0, 1, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 1] = CreateNewHexagon(1, 1, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 1] = CreateNewHexagon(2, 1, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 1] = CreateNewHexagon(3, 1, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 1] = CreateNewHexagon(4, 1, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 1] = CreateNewHexagon(5, 1, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 1] = CreateNewHexagon(6, 1, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 1] = CreateNewHexagon(7, 1, HexagonType.Yellow, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 2] = CreateNewHexagon(0, 2, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 2] = CreateNewHexagon(1, 2, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 2] = CreateNewHexagon(2, 2, HexagonType.Green, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 2] = CreateNewHexagon(3, 2, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 2] = CreateNewHexagon(4, 2, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 2] = CreateNewHexagon(5, 2, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 2] = CreateNewHexagon(6, 2, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 2] = CreateNewHexagon(7, 2, HexagonType.Blue, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 3] = CreateNewHexagon(0, 3, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 3] = CreateNewHexagon(1, 3, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 3] = CreateNewHexagon(2, 3, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 3] = CreateNewHexagon(3, 3, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 3] = CreateNewHexagon(4, 3, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 3] = CreateNewHexagon(5, 3, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 3] = CreateNewHexagon(6, 3, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 3] = CreateNewHexagon(7, 3, HexagonType.Cyan, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 4] = CreateNewHexagon(0, 4, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 4] = CreateNewHexagon(1, 4, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 4] = CreateNewHexagon(2, 4, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 4] = CreateNewHexagon(3, 4, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 4] = CreateNewHexagon(4, 4, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 4] = CreateNewHexagon(5, 4, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 4] = CreateNewHexagon(6, 4, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 4] = CreateNewHexagon(7, 4, HexagonType.Yellow, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 5] = CreateNewHexagon(0, 5, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 5] = CreateNewHexagon(1, 5, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 5] = CreateNewHexagon(2, 5, HexagonType.Green, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 5] = CreateNewHexagon(3, 5, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 5] = CreateNewHexagon(4, 5, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 5] = CreateNewHexagon(5, 5, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 5] = CreateNewHexagon(6, 5, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 5] = CreateNewHexagon(7, 5, HexagonType.Cyan, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 6] = CreateNewHexagon(0, 6, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 6] = CreateNewHexagon(1, 6, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 6] = CreateNewHexagon(2, 6, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 6] = CreateNewHexagon(3, 6, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 6] = CreateNewHexagon(4, 6, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 6] = CreateNewHexagon(5, 6, HexagonType.Green, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 6] = CreateNewHexagon(6, 6, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 6] = CreateNewHexagon(7, 6, HexagonType.Yellow, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 7] = CreateNewHexagon(0, 7, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 7] = CreateNewHexagon(1, 7, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 7] = CreateNewHexagon(2, 7, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 7] = CreateNewHexagon(3, 7, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 7] = CreateNewHexagon(4, 7, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 7] = CreateNewHexagon(5, 7, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 7] = CreateNewHexagon(6, 7, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 7] = CreateNewHexagon(7, 7, HexagonType.Blue, hexagonProperties.DefaultHexagon);

            hexagonGrid[0, 8] = CreateNewHexagon(0, 8, HexagonType.Yellow, hexagonProperties.DefaultHexagon);
            hexagonGrid[1, 8] = CreateNewHexagon(1, 8, HexagonType.Purple, hexagonProperties.DefaultHexagon);
            hexagonGrid[2, 8] = CreateNewHexagon(2, 8, HexagonType.Green, hexagonProperties.DefaultHexagon);
            hexagonGrid[3, 8] = CreateNewHexagon(3, 8, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[4, 8] = CreateNewHexagon(4, 8, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[5, 8] = CreateNewHexagon(5, 8, HexagonType.Blue, hexagonProperties.DefaultHexagon);
            hexagonGrid[6, 8] = CreateNewHexagon(6, 8, HexagonType.Red, hexagonProperties.DefaultHexagon);
            hexagonGrid[7, 8] = CreateNewHexagon(7, 8, HexagonType.Cyan, hexagonProperties.DefaultHexagon);
        }

        private Hexagon CreateNewHexagon(int width, int height, HexagonType hexagonType, Hexagon hexagonPrefab)
        {
            var hexPosition = gridSpawner.GetHexagonWorldPosition(width, height);
            var hexagon = Object.Instantiate(hexagonProperties.DefaultHexagon, hexPosition, Quaternion.identity);
            hexagon.InitializeForTest(width, height, hexagonType);
            return hexagon;
        }
    }
}