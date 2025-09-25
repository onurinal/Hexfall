using System.Collections;
using Hexfall.CameraManager;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Manager;
using Hexfall.Player;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridMovement
    {
        private LevelManager levelManager;
        private GridSpawner gridSpawner;
        private GridChecker gridChecker;
        private Hexagon[,] hexagonGrid;
        private HexagonProperties hexagonProperties;
        private PlayerHighlight playerHighlight;
        private CameraController cameraController;

        private IEnumerator fillHexagonsEmptySlotCoroutine;
        private IEnumerator swapHexagonsCoroutine;
        private IEnumerator fallHexagonsCoroutine;

        private int gridWidth, gridHeight;

        public void Initialize(Hexagon[,] hexagonGrid, GridSpawner gridSpawner, LevelManager levelManager, GridChecker gridChecker, LevelProperties levelProperties,
        HexagonProperties hexagonProperties,
        CameraController cameraController)
        {
            this.hexagonGrid = hexagonGrid;
            this.gridSpawner = gridSpawner;
            this.levelManager = levelManager;
            this.gridChecker = gridChecker;
            this.hexagonProperties = hexagonProperties;
            this.cameraController = cameraController;

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;
        }

        public void InitializePlayerHighlight(PlayerHighlight playerHighlight)
        {
            this.playerHighlight = playerHighlight;
        }

        private IEnumerator SwapHexagonsCoroutine(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, Vector2 moveDirection, Vector2 currentInputPosition)
        {
            if (firstHex == null || secondHex == null || thirdHex == null) yield break;

            var centerPosition = (firstHex.transform.position + secondHex.transform.position + thirdHex.transform.position) / 3f;
            var isInputLeftSide = currentInputPosition.x < centerPosition.x;
            var isInputTopSide = currentInputPosition.y > centerPosition.y;

            if ((isInputLeftSide && moveDirection == Vector2.up) || (isInputTopSide && moveDirection == Vector2.right) || (!isInputTopSide && moveDirection == Vector2.left) ||
                (!isInputLeftSide && moveDirection == Vector2.down))
            {
                for (int i = 0; i < 3; i++)
                {
                    SwapThreeHexagons(firstHex, secondHex, thirdHex, centerPosition, -hexagonProperties.RotationAngle);
                    playerHighlight.StartRotateCoroutine(centerPosition, -hexagonProperties.RotationAngle);
                    yield return new WaitForSeconds(hexagonProperties.MoveDuration);
                    if (gridChecker.ScanGridAndGetMatchListCount() > 0)
                    {
                        playerHighlight.HideHighlight();
                        yield return CoroutineHandler.Instance.StartCoroutine(levelManager.StartScanGrid());
                        EventManager.StartOnMoveChangedEvent();
                        break;
                    }
                }
            }
            else if ((isInputLeftSide && moveDirection == Vector2.down) || (isInputTopSide && moveDirection == Vector2.left) ||
                     (!isInputTopSide && moveDirection == Vector2.right) ||
                     (!isInputLeftSide && moveDirection == Vector2.up))
            {
                for (int i = 0; i < 3; i++)
                {
                    SwapThreeHexagons(firstHex, thirdHex, secondHex, centerPosition, hexagonProperties.RotationAngle);
                    playerHighlight.StartRotateCoroutine(centerPosition, hexagonProperties.RotationAngle);
                    yield return new WaitForSeconds(hexagonProperties.MoveDuration);
                    if (gridChecker.ScanGridAndGetMatchListCount() > 0)
                    {
                        playerHighlight.HideHighlight();
                        yield return CoroutineHandler.Instance.StartCoroutine(levelManager.StartScanGrid());
                        EventManager.StartOnMoveChangedEvent();
                        break;
                    }
                }
            }

            swapHexagonsCoroutine = null;
        }

        private void SwapThreeHexagons(Hexagon hexagon1, Hexagon hexagon2, Hexagon hexagon3, Vector3 centerPosition, float angle)
        {
            if (hexagon1 == null || hexagon2 == null || hexagon3 == null) return;

            var tempPosition = hexagon1.transform.position;
            hexagon1.StartRotateCoroutine(centerPosition, hexagon2.transform.position, angle);
            hexagon2.StartRotateCoroutine(centerPosition, hexagon3.transform.position, angle);
            hexagon3.StartRotateCoroutine(centerPosition, tempPosition, angle);

            var tempX = hexagon1.IndexX;
            var tempY = hexagon1.IndexY;
            hexagon1.SetIndices(hexagon2.IndexX, hexagon2.IndexY);
            hexagon2.SetIndices(hexagon3.IndexX, hexagon3.IndexY);
            hexagon3.SetIndices(tempX, tempY);

            hexagonGrid[hexagon1.IndexX, hexagon1.IndexY] = hexagon1;
            hexagonGrid[hexagon2.IndexX, hexagon2.IndexY] = hexagon2;
            hexagonGrid[hexagon3.IndexX, hexagon3.IndexY] = hexagon3;
        }

        public IEnumerator StartSwapHexagons(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, Vector2 moveDirection, Vector2 currentInputPosition)
        {
            if (swapHexagonsCoroutine != null) yield break;
            swapHexagonsCoroutine = SwapHexagonsCoroutine(firstHex, secondHex, thirdHex, moveDirection, currentInputPosition);
            yield return swapHexagonsCoroutine;
        }

        public void StopSwapHexagons()
        {
            if (swapHexagonsCoroutine == null) return;
            CoroutineHandler.Instance.StopCoroutine(swapHexagonsCoroutine);
            swapHexagonsCoroutine = null;
        }

        private IEnumerator FillHexagonEmptySlotCoroutine(float moveDuration)
        {
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
                                var targetPosition = gridSpawner.GetHexagonWorldPosition(width, height);
                                hexagonGrid[width, i].Move(targetPosition, moveDuration);
                                hexagonGrid[width, i].SetIndices(width, height);
                                hexagonGrid[width, height] = hexagonGrid[width, i];
                                hexagonGrid[width, i] = null;
                                break;
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(moveDuration / 4f);
            }

            yield return new WaitForSeconds(moveDuration - (moveDuration / 4f));

            fillHexagonsEmptySlotCoroutine = null;
        }

        public IEnumerator StartFillHexagonEmptySlot(float moveDuration)
        {
            if (fillHexagonsEmptySlotCoroutine != null) yield break;
            fillHexagonsEmptySlotCoroutine = FillHexagonEmptySlotCoroutine(moveDuration);
            yield return fillHexagonsEmptySlotCoroutine;
        }

        public void StopFillHexagonEmptySlot()
        {
            if (fillHexagonsEmptySlotCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(fillHexagonsEmptySlotCoroutine);
            fillHexagonsEmptySlotCoroutine = null;
        }

        private IEnumerator FallHexagonsCoroutine(float moveDuration)
        {
            for (int width = 0; width < gridWidth; width++)
            {
                for (int height = 0; height < gridHeight; height++)
                {
                    var targetPosition = gridSpawner.GetHexagonWorldPosition(width, height);
                    hexagonGrid[width, height].Move(targetPosition, moveDuration);
                    yield return new WaitForSeconds(moveDuration / 6f);
                }

                yield return new WaitForSeconds(moveDuration / 10f);
            }

            var gap = moveDuration / 6f + moveDuration / 10f;
            yield return new WaitForSeconds(moveDuration - gap);

            fallHexagonsCoroutine = null;
        }

        public IEnumerator StartFallHexagonsCoroutine(float spawnGapBetweenColumns)
        {
            if (fallHexagonsCoroutine != null) yield break;

            fallHexagonsCoroutine = FallHexagonsCoroutine(spawnGapBetweenColumns);
            yield return fallHexagonsCoroutine;
        }

        private IEnumerator StopFallHexagonsCoroutine()
        {
            if (fallHexagonsCoroutine == null) yield break;

            CoroutineHandler.Instance.StopCoroutine(fallHexagonsCoroutine);
            fallHexagonsCoroutine = null;
        }

        public void MoveAllHexagonsToTheTop()
        {
            for (int width = 0; width < gridWidth; width++)
            {
                for (int height = 0; height < gridHeight; height++)
                {
                    var targetPositionX = gridSpawner.GetHexagonWorldPosition(width, height).x;
                    var targetPositionY = cameraController.GetTopLeftWorldPosition().y;
                    hexagonGrid[width, height].Move(new Vector2(targetPositionX, targetPositionY), 0f);
                }
            }
        }

        public void DestroyAllHexagonsWhenCountdownOver()
        {
            foreach (var hexagon in hexagonGrid)
            {
                hexagon.DestroyHexagon(hexagonProperties.DestroyDuration);
            }
        }
    }
}