using System.Collections;
using System.Collections.Generic;
using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Manager;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float SwapThreshold = 0.2f;

        private Hexagon firstHexagon, secondHexagon, thirdHexagon;
        private Vector2Int selectedHexagonAxis, secondHexagonAxis, thirdHexagonAxis;
        private GridSpawner gridSpawner;
        private PlayerHighlight playerHighlight;
        private PlayerInput playerInput;
        private IGridPlayerMovement gridPlayerMovement;

        private bool canSelectHex;
        private bool isSwapping;
        private bool canSwap = true;
        private IEnumerator swapCoroutine;
        private readonly Vector2Int[][] neighbourOffsets = new Vector2Int[][]
        {
            new Vector2Int[] { new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, 0), new(1, 1), }, // for even rows
            new Vector2Int[] { new(-1, -1), new(-1, 0), new(0, -1), new(0, 1), new(1, 0), new(1, -1), } // for odd rows
        };

        private int gridWidth, gridHeight;

        public void Initialize(GridSpawner gridSpawner, IGridPlayerMovement gridPlayerMovement, PlayerHighlight playerHighlight, PlayerInput playerInput, LevelProperties levelProperties)
        {
            this.gridSpawner = gridSpawner;
            this.gridPlayerMovement = gridPlayerMovement;
            this.playerInput = playerInput;
            this.playerHighlight = playerHighlight;

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;

            EventManager.OnSwapped += RestorePreviousHexagons;
        }

        private void OnDestroy()
        {
            EventManager.OnSwapped -= RestorePreviousHexagons;
        }

        public void HandleHexagonSelect()
        {
            var inputPosition = GetInputPosition();
            if (inputPosition == Vector2.zero) return;

            if (canSelectHex)
            {
                canSelectHex = false;
                SelectNewHexagons(inputPosition);
                return;
            }

            if (canSwap && firstHexagon != null)
            {
                AttemptSwap(inputPosition);
            }
        }

        private void SelectNewHexagons(Vector2 inputPosition)
        {
            firstHexagon = GetHexagonAtInput(inputPosition);
            (secondHexagon, thirdHexagon) = FindAdjacentHexagons(inputPosition);
        }

        private Vector2 GetInputPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerInput.FirstMousePosition = playerInput.CurrentMousePosition;
                return playerInput.FirstMousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                return playerInput.CurrentMousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 delta = playerInput.CurrentMousePosition - playerInput.FirstMousePosition;

                // if player move the input after select hex then don't allow to select again
                if (delta.magnitude < 0.1f && !isSwapping)
                {
                    canSelectHex = true;
                    return playerInput.CurrentMousePosition;
                }
                else
                {
                    canSwap = true;
                    return Vector2.zero;
                }
            }

            return Vector2.zero;
        }

        private void AttemptSwap(Vector2 currentInputPosition)
        {
            if (currentInputPosition == Vector2.zero || firstHexagon == null || secondHexagon == null || thirdHexagon == null) return;

            var direction = GetMovementDirection(currentInputPosition);
            if (direction.magnitude > SwapThreshold && !isSwapping)
            {
                isSwapping = true;
                canSwap = false;
                EventManager.StartOnSwappingEvent();
                SavePreviousHexagonAxisAfterSwapped();
                StartSwap(direction);
            }
        }

        private IEnumerator SwapCoroutine(Vector2 direction)
        {
            yield return gridPlayerMovement.StartSwapHexagons(firstHexagon, secondHexagon, thirdHexagon, direction);
            isSwapping = false;
            swapCoroutine = null;
        }

        private void StartSwap(Vector2 direction)
        {
            if (swapCoroutine != null) return;
            swapCoroutine = SwapCoroutine(direction);
            CoroutineHandler.Instance.StartCoroutine(swapCoroutine);
        }

        private void StopSwap()
        {
            if (swapCoroutine == null) return;
            CoroutineHandler.Instance.StopCoroutine(swapCoroutine);
            swapCoroutine = null;
        }

        private Vector2 GetMovementDirection(Vector2 currentInputPosition)
        {
            var delta = currentInputPosition - playerInput.FirstMousePosition;

            if (delta.magnitude < SwapThreshold) return Vector2.zero;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                return delta.x > 0 ? Vector2.right : Vector2.left; // if delta x positive then move to clockwise
            }

            return delta.y > 0 ? Vector2.up : Vector2.down;
        }

        private (Hexagon, Hexagon) FindAdjacentHexagons(Vector2 inputPosition)
        {
            if (firstHexagon == null || inputPosition == Vector2.zero) return (null, null);

            Vector2 hexCenter = firstHexagon.transform.position;
            var angle = GetAngle(hexCenter, inputPosition);

            (secondHexagon, thirdHexagon) = GetOtherTwoHexagons(angle);

            return (secondHexagon, thirdHexagon);
        }

        private (Hexagon, Hexagon) GetOtherTwoHexagons(float inputAngle)
        {
            var neighbours = GetValidNeighbourHexagons(firstHexagon.IndexX, firstHexagon.IndexY);
            if (neighbours.Count < 2) return (null, null);

            var (secondHexAxis, thirdHexAxis) = FindTwoClosestNeighbours(neighbours, inputAngle);
            secondHexagon = gridSpawner.GetHexagonAtAxis(secondHexAxis.x, secondHexAxis.y);
            thirdHexagon = gridSpawner.GetHexagonAtAxis(thirdHexAxis.x, thirdHexAxis.y);

            if (secondHexagon != null && thirdHexagon != null)
            {
                playerHighlight.DrawHexOutline(firstHexagon, secondHexagon);
            }

            return (secondHexagon, thirdHexagon);
        }

        private List<Vector2Int> GetValidNeighbourHexagons(int indexX, int indexY)
        {
            List<Vector2Int> validNeighbours = new List<Vector2Int>();
            int rowType = indexX % 2; // 0 for even, 1 for odd

            foreach (var offset in neighbourOffsets[rowType])
            {
                Vector2Int neighbour = new Vector2Int(indexX + offset.x, indexY + offset.y);
                if (IsHexagonWithinBounds(neighbour))
                {
                    validNeighbours.Add(neighbour);
                }
            }

            return validNeighbours;
        }

        private bool IsHexagonWithinBounds(Vector2Int hexagon)
        {
            return hexagon.x >= 0 && hexagon.y >= 0 && hexagon.x < gridWidth && hexagon.y < gridHeight;
        }

        private (Vector2Int, Vector2Int) FindTwoClosestNeighbours(List<Vector2Int> neighbours, float inputAngle)
        {
            Vector2Int closest1 = Vector2Int.zero, closest2 = Vector2Int.zero;
            float minDiff = float.MaxValue, minDiff2 = float.MaxValue;

            // Find the closest hexagon to the input angle
            foreach (var neighbour in neighbours)
            {
                var targetPosition = gridSpawner.GetHexagonWorldPosition(neighbour.x, neighbour.y);
                var angle = GetAngle(firstHexagon.transform.position, targetPosition);
                var angleDiff = Mathf.Abs(Mathf.DeltaAngle(inputAngle, angle));

                if (angleDiff < minDiff) // new closest one
                {
                    minDiff = angleDiff;
                    closest1 = neighbour;
                }
            }

            neighbours.Remove(closest1);
            var closest1Neighbours = GetValidNeighbourHexagons(closest1.x, closest1.y);

            // Find the second-closest hexagon that is also a neighbor of the first closest hex
            foreach (var neighbour in neighbours)
            {
                if (!closest1Neighbours.Contains(neighbour)) continue;

                var targetPosition = gridSpawner.GetHexagonWorldPosition(neighbour.x, neighbour.y);
                var angle = GetAngle(firstHexagon.transform.position, targetPosition);
                var angleDiff = Mathf.Abs(Mathf.DeltaAngle(inputAngle, angle));

                if (angleDiff < minDiff2)
                {
                    minDiff2 = angleDiff;
                    closest2 = neighbour;
                }
            }

            return EnsureClosestHexagonsClockwiseOrder(closest1, closest2);
        }

        private (Vector2Int, Vector2Int) EnsureClosestHexagonsClockwiseOrder(Vector2Int closest1, Vector2Int closest2)
        {
            var angle1 = GetAngle(firstHexagon.transform.position, gridSpawner.GetHexagonWorldPosition(closest1.x, closest1.y));
            var angle2 = GetAngle(firstHexagon.transform.position, gridSpawner.GetHexagonWorldPosition(closest2.x, closest2.y));

            var angleDiff = Mathf.DeltaAngle(angle1, angle2);

            if (angleDiff < 0)
            {
                // Swap to make closest1 the clockwise neighbor
                (closest1, closest2) = (closest2, closest1);
            }

            return (closest1, closest2);
        }

        private Hexagon GetHexagonAtInput(Vector2 inputPosition)
        {
            var hit = Physics2D.Raycast(inputPosition, Vector2.zero);

            if (!hit.collider) return null;

            var hexagon = hit.collider.gameObject.GetComponentInParent<Hexagon>();
            return hexagon;
        }

        public float GetAngle(Vector2 center, Vector2 target)
        {
            Vector2 diff = target - center;
            float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
            angle = (angle < 0) ? angle + 360 : angle;
            return angle;
        }

        private void RestorePreviousHexagons()
        {
            firstHexagon = gridSpawner.GetHexagonAtAxis(selectedHexagonAxis.x, selectedHexagonAxis.y);
            secondHexagon = gridSpawner.GetHexagonAtAxis(secondHexagonAxis.x, secondHexagonAxis.y);
            thirdHexagon = gridSpawner.GetHexagonAtAxis(thirdHexagonAxis.x, thirdHexagonAxis.y);
        }

        private void SavePreviousHexagonAxisAfterSwapped()
        {
            selectedHexagonAxis = new Vector2Int(firstHexagon.IndexX, firstHexagon.IndexY);
            secondHexagonAxis = new Vector2Int(secondHexagon.IndexX, secondHexagon.IndexY);
            thirdHexagonAxis = new Vector2Int(thirdHexagon.IndexX, thirdHexagon.IndexY);
        }
    }
}