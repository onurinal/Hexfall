using System.Collections;
using DefaultNamespace;
using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridMovement
    {
        private GridSpawner gridSpawner;
        private Hexagon[,] hexagonGrid;
        private LevelProperties levelProperties;
        private HexagonProperties hexagonProperties;

        private IEnumerator fillHexagonsEmptySlotCoroutine;

        public void Initialize(Hexagon[,] hexagonGrid, GridSpawner gridSpawner, LevelProperties levelProperties, HexagonProperties hexagonProperties)
        {
            this.hexagonGrid = hexagonGrid;
            this.gridSpawner = gridSpawner;
            this.levelProperties = levelProperties;
            this.hexagonProperties = hexagonProperties;
        }

        private IEnumerator FillHexagonEmptySlotCoroutine()
        {
            var gridWidth = levelProperties.GridWidth;
            var gridHeight = levelProperties.GridHeight;

            for (int width = 0; width < gridWidth; width++)
            {
                for (int height = 0; height < gridHeight; height++)
                {
                    if (hexagonGrid[width, height] == null)
                    {
                        for (int i = height; i < gridHeight; i++)
                        {
                            if (hexagonGrid[width, i] != null)
                            {
                                hexagonGrid[width, i].SetIndices(width, height);
                                hexagonGrid[width, height] = hexagonGrid[width, i];
                                var targetPosition = gridSpawner.GetHexagonWorldPosition(width, height);
                                hexagonGrid[width, i].Move(targetPosition);
                                hexagonGrid[width, height].UpdateIndexText(); // REMOVE IT AFTER TESTING FEATURES, DON'T NEED
                                hexagonGrid[width, i] = null;
                                break;
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(hexagonProperties.MoveDuration / 4f);
            }

            fillHexagonsEmptySlotCoroutine = null;
        }

        public IEnumerator StartFillHexagonEmptySlot()
        {
            if (fillHexagonsEmptySlotCoroutine != null) yield break;
            fillHexagonsEmptySlotCoroutine = FillHexagonEmptySlotCoroutine();
            yield return CoroutineHandler.Instance.StartCoroutine(fillHexagonsEmptySlotCoroutine);
        }

        public void StopFillHexagonEmptySlot()
        {
            if (fillHexagonsEmptySlotCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(fillHexagonsEmptySlotCoroutine);
            fillHexagonsEmptySlotCoroutine = null;
        }
    }
}