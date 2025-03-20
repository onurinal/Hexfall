using System.Collections.Generic;
using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerSelection
    {
        private Hexagon selectedHexagon;
        private GridSpawner gridSpawner;
        private PlayerHighlight playerHighlight;
        private PlayerInput playerInput;

        private bool isHexagonSelected = false;
        private readonly Vector2Int[][] neighbourOffsets = new Vector2Int[][]
        {
            new Vector2Int[] { new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, 0), new(1, 1), }, // for even rows
            new Vector2Int[] { new(-1, -1), new(-1, 0), new(0, -1), new(0, 1), new(1, 0), new(1, -1), } // for odd rows
        };

        private int gridWidth, gridHeight;

        public void Initialize(GridSpawner gridSpawner, PlayerInput playerInput, LevelProperties levelProperties, SpriteRenderer groupHighlightSprite)
        {
            this.gridSpawner = gridSpawner;
            this.playerInput = playerInput;
            playerHighlight = new PlayerHighlight();
            playerHighlight.Initialize(this, groupHighlightSprite);

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;
        }

        public void HandleHexagonSelect()
        {
            var inputPosition = GetInputPosition();
            if (inputPosition != Vector2.zero)
            {
                if (!selectedHexagon)
                {
                    selectedHexagon = GetHexagonAtInput(inputPosition);
                    FindSelectedAreaOfHexagonAtInputPosition(inputPosition);

                    if (selectedHexagon)
                    {
                        // Debug.Log($"Selected Hexagon Type: {selectedHexagon.HexagonType}, IndexX : {selectedHexagon.IndexX}, IndexY : {selectedHexagon.IndexY}");
                    }
                }
            }
        }

        private Vector2 GetInputPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerInput.FirstMousePosition = playerInput.CurrentMousePosition;
                return playerInput.FirstMousePosition;
            }

            if (Input.GetMouseButton(0) && selectedHexagon)
            {
                if (selectedHexagon != null)
                {
                    // Debug.Log($"Still selecting Hexagon Type: {selectedHexagon.HexagonType}, IndexX : {selectedHexagon.IndexX}, IndexY : {selectedHexagon.IndexY}");
                    return playerInput.CurrentMousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0) && selectedHexagon)
            {
                selectedHexagon = null;
                return Vector2.zero;
            }

            return Vector2.zero;
        }

        private Hexagon GetHexagonAtInput(Vector2 inputPosition)
        {
            var hit = Physics2D.Raycast(inputPosition, Vector2.down);

            if (!hit.collider) return null;

            var hexagon = hit.collider.gameObject.GetComponentInParent<Hexagon>();
            return hexagon;
        }

        /// <summary>
        /// the method will calculate the area of hexagon which we need this for select other two hexagon
        /// this method taking the user input and for example: if user input is low than world position of selected hexagon 
        /// then it will try to select bottom two hexagon, if input is higher than world position of selected hexagon
        /// then it will try to select top two hexagon. The method basically working like this
        /// </summary>
        /// <param name="inputPosition"></param>
        private void FindSelectedAreaOfHexagonAtInputPosition(Vector2 inputPosition)
        {
            if (!selectedHexagon || inputPosition == Vector2.zero) return;

            // a box collider was placed inside the hexagon. The box collider was divided into 8 parts and these parts are in the shape of triangles
            // to find the center position of this user input, we need collider bound size and divide this variable by 2

            Vector2 hexCenter = selectedHexagon.transform.position;
            var angle = GetAngle(hexCenter, inputPosition);

            CalculateOtherTwoHexagons(angle);
        }

        private void CalculateOtherTwoHexagons(float inputAngle)
        {
            var neighbours = GetValidNeighbourHexagons(selectedHexagon.IndexX, selectedHexagon.IndexY);
            if (neighbours.Count < 2) return;

            var (secondHexAxis, thirdHexAxis) = GetClosestNeighbour(neighbours, inputAngle);

            var secondHex = gridSpawner.GetHexagonObject(secondHexAxis.x, secondHexAxis.y);
            var thirdHex = gridSpawner.GetHexagonObject(thirdHexAxis.x, thirdHexAxis.y);

            playerHighlight.DrawHexOutline(selectedHexagon, secondHex);

            Debug.Log($"First Hex: {selectedHexagon.IndexX}, {selectedHexagon.IndexY} Second Hex: {secondHex.IndexX}, {secondHex.IndexY} Third Hex: {thirdHex.IndexX}, {thirdHex.IndexY} ");
        }

        private List<Vector2Int> GetValidNeighbourHexagons(int indexX, int indexY)
        {
            List<Vector2Int> validNeighbours = new List<Vector2Int>();
            int rowType = indexX % 2; // 0 for even, 1 for odd

            foreach (var offset in neighbourOffsets[rowType])
            {
                Vector2Int neighbour = new Vector2Int(indexX + offset.x, indexY + offset.y);
                if (IsValidHexagon(neighbour))
                {
                    validNeighbours.Add(neighbour);
                }
            }

            return validNeighbours;
        }

        private bool IsValidHexagon(Vector2Int hexagon)
        {
            return hexagon.x >= 0 && hexagon.y >= 0 && hexagon.x < gridWidth && hexagon.y < gridHeight;
        }

        private (Vector2Int, Vector2Int) GetClosestNeighbour(List<Vector2Int> neighbours, float inputAngle)
        {
            Vector2Int closest1 = Vector2Int.zero, closest2 = Vector2Int.zero;
            float minDiff = float.MaxValue, minDiff2 = float.MaxValue;

            // Find the closest hexagon to the input angle
            foreach (var neighbour in neighbours)
            {
                var targetPosition = gridSpawner.GetHexagonWorldPosition(neighbour.x, neighbour.y);
                var angle = GetAngle(selectedHexagon.transform.position, targetPosition);
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
                var angle = GetAngle(selectedHexagon.transform.position, targetPosition);
                var angleDiff = Mathf.Abs(Mathf.DeltaAngle(inputAngle, angle));

                if (angleDiff < minDiff2)
                {
                    minDiff2 = angleDiff;
                    closest2 = neighbour;
                }
            }

            (closest1, closest2) = EnsureClosestHexagonsClockwiseOrder(closest1, closest2);
            return (closest1, closest2);
        }

        private (Vector2Int, Vector2Int) EnsureClosestHexagonsClockwiseOrder(Vector2Int closest1, Vector2Int closest2)
        {
            if (closest1 == Vector2Int.zero || closest2 == Vector2Int.zero) return (closest1, closest2);

            var angle1 = GetAngle(selectedHexagon.transform.position, gridSpawner.GetHexagonWorldPosition(closest1.x, closest1.y));
            var angle2 = GetAngle(selectedHexagon.transform.position, gridSpawner.GetHexagonWorldPosition(closest2.x, closest2.y));

            var angleDiff = Mathf.DeltaAngle(angle1, angle2);

            if (angleDiff < 0)
            {
                // Swap to make closest1 the clockwise neighbor
                (closest1, closest2) = (closest2, closest1);
            }

            return (closest1, closest2);
        }

        public float GetAngle(Vector2 center, Vector2 target)
        {
            Vector2 diff = target - center;
            float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
            angle = (angle < 0) ? angle + 360 : angle;
            return angle;
        }
    }
}