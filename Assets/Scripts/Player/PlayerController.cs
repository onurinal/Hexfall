using System.Collections.Generic;
using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private GridSpawner gridSpawner;

        private int gridWidth, gridHeight;
        private Hexagon selectedHexagon;
        private bool isHexagonSelected = false;

        private readonly Vector2Int[][] neighbourOffsets = new Vector2Int[][]
        {
            new Vector2Int[] { new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, 0), new(1, 1), }, // for even rows
            new Vector2Int[] { new(-1, -1), new(-1, 0), new(0, -1), new(0, 1), new(1, 0), new(1, -1), } // for odd rows
        };

        public void Initialize(GridSpawner gridSpawner, LevelProperties levelProperties)
        {
            playerInput.Initialize();
            this.gridSpawner = gridSpawner;

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;
        }

        private void Update()
        {
            playerInput.UpdatePlayerInput();
            HandleHexagonSelect();
        }

        private void HandleHexagonSelect()
        {
            var inputPosition = GetInputPosition();
            if (inputPosition != Vector2.zero)
            {
                if (!selectedHexagon)
                {
                    selectedHexagon = GetHexagonAtInputPosition(inputPosition);
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

        private Hexagon GetHexagonAtInputPosition(Vector2 inputPosition)
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
            Debug.Log($"selected angle: {angle}");

            CalculateOtherTwoHexagons(angle);
        }

        private void CalculateOtherTwoHexagons(float inputAngle)
        {
            Vector2 hexCenter = selectedHexagon.transform.position;

            var neighbours = GetValidNeighbourHexagons(selectedHexagon.IndexX, selectedHexagon.IndexY);
            if (neighbours.Count < 2) return;

            var closestTwoHexagons = GetClosestNeighbour(neighbours, inputAngle);

            var secondHex = gridSpawner.GetHexagonObject(closestTwoHexagons[0].x, closestTwoHexagons[0].y);
            var thirdHex = gridSpawner.GetHexagonObject(closestTwoHexagons[1].x, closestTwoHexagons[1].y);

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

        private Vector2Int[] GetClosestNeighbour(List<Vector2Int> neighbours, float inputAngle)
        {
            Vector2Int closest1 = Vector2Int.zero, closest2 = Vector2Int.zero;
            float minDiff = float.MaxValue, minDiff2 = float.MaxValue;
            var selectedHexPos = selectedHexagon.transform.position;

            foreach (var neighbour in neighbours)
            {
                var targetPosition = gridSpawner.GetHexagonWorldPosition(neighbour.x, neighbour.y);
                var angle = GetAngle(selectedHexPos, targetPosition);
                var angleDiff = Mathf.Abs(Mathf.DeltaAngle(inputAngle, angle));

                if (angleDiff < minDiff) // new closest one
                {
                    minDiff = angleDiff;
                    closest1 = neighbour;
                }
            }

            neighbours.Remove(closest1);

            var closest1Neighbours = GetValidNeighbourHexagons(closest1.x, closest1.y);

            foreach (var neighbour in neighbours)
            {
                if (closest1Neighbours.Contains(neighbour))
                {
                    var targetPosition = gridSpawner.GetHexagonWorldPosition(neighbour.x, neighbour.y);
                    var targetAngle = GetAngle(selectedHexPos, targetPosition);
                    var angleDiff = Mathf.Abs(Mathf.DeltaAngle(inputAngle, targetAngle));

                    if (angleDiff < minDiff2)
                    {
                        minDiff2 = angleDiff;
                        closest2 = neighbour;
                    }
                }
            }

            return new Vector2Int[] { closest1, closest2 };
        }

        private float GetAngle(Vector2 center, Vector2 target)
        {
            Vector2 diff = new Vector2(target.x - center.x, target.y - center.y);
            float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }
    }
}