using System.Collections;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Manager;
using Hexfall.Player;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridMovement : IGridPlayerMovement
    {
        private LevelManager levelManager;
        private GridSpawner gridSpawner;
        private GridChecker gridChecker;
        private PlayerHighlight playerHighlight;
        private Hexagon[,] hexagonGrid;
        private LevelProperties levelProperties;
        private HexagonProperties hexagonProperties;

        private IEnumerator fillHexagonsEmptySlotCoroutine;

        private IEnumerator swapHexagonsCoroutine;
        public bool IsSwapping { get; set; } = false;

        public void Initialize(Hexagon[,] hexagonGrid, GridSpawner gridSpawner, LevelManager levelManager, GridChecker gridChecker, LevelProperties levelProperties, HexagonProperties hexagonProperties)
        {
            this.hexagonGrid = hexagonGrid;
            this.gridSpawner = gridSpawner;
            this.levelManager = levelManager;
            this.gridChecker = gridChecker;
            this.levelProperties = levelProperties;
            this.hexagonProperties = hexagonProperties;
        }

        private IEnumerator SwapHexagonsCoroutine(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, Vector2 moveDirection)
        {
            if (firstHex == null || secondHex == null || thirdHex == null) yield break;

            if (moveDirection == Vector2.up || moveDirection == Vector2.left)
            {
                for (int i = 0; i < 3; i++)
                {
                    SwapThreeHexagons(firstHex, secondHex, thirdHex);
                    yield return new WaitForSeconds(hexagonProperties.MoveDuration);
                    if (gridChecker.ScanGridAndGetMatchListCount() > 0)
                    {
                        yield return CoroutineHandler.Instance.StartCoroutine(levelManager.StartScanGrid());
                        break;
                    }
                }
            }
            else if (moveDirection == Vector2.down || moveDirection == Vector2.right)
            {
                for (int i = 0; i < 3; i++)
                {
                    SwapThreeHexagons(firstHex, thirdHex, secondHex);
                    yield return new WaitForSeconds(hexagonProperties.MoveDuration);
                    if (gridChecker.ScanGridAndGetMatchListCount() > 0)
                    {
                        yield return CoroutineHandler.Instance.StartCoroutine(levelManager.StartScanGrid());
                        break;
                    }
                }
            }

            IsSwapping = false;
            EventManager.StartOnSwappedEvent();
            swapHexagonsCoroutine = null;
        }

        private void SwapThreeHexagons(Hexagon hexagon1, Hexagon hexagon2, Hexagon hexagon3)
        {
            if (hexagon1 == null || hexagon2 == null || hexagon3 == null) return;

            var tempX = hexagon1.IndexX;
            var tempY = hexagon1.IndexY;
            hexagon1.SetIndices(hexagon2.IndexX, hexagon2.IndexY);
            hexagon2.SetIndices(hexagon3.IndexX, hexagon3.IndexY);
            hexagon3.SetIndices(tempX, tempY);

            var tempPosition = hexagon1.transform.position;
            hexagon1.Move(hexagon2.transform.position);
            hexagon2.Move(hexagon3.transform.position);
            hexagon3.Move(tempPosition);

            hexagonGrid[hexagon1.IndexX, hexagon1.IndexY] = hexagon1;
            hexagonGrid[hexagon2.IndexX, hexagon2.IndexY] = hexagon2;
            hexagonGrid[hexagon3.IndexX, hexagon3.IndexY] = hexagon3;
        }

        public IEnumerator StartSwapHexagons(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, Vector2 moveDirection)
        {
            if (swapHexagonsCoroutine != null) yield break;
            swapHexagonsCoroutine = SwapHexagonsCoroutine(firstHex, secondHex, thirdHex, moveDirection);
            yield return CoroutineHandler.Instance.StartCoroutine(swapHexagonsCoroutine);
        }

        public void StopSwapHexagons()
        {
            if (swapHexagonsCoroutine == null) return;
            CoroutineHandler.Instance.StopCoroutine(swapHexagonsCoroutine);
            swapHexagonsCoroutine = null;
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
            yield return fillHexagonsEmptySlotCoroutine;
        }

        public void StopFillHexagonEmptySlot()
        {
            if (fillHexagonsEmptySlotCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(fillHexagonsEmptySlotCoroutine);
            fillHexagonsEmptySlotCoroutine = null;
        }
    }
}