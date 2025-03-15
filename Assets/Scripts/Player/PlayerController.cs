using Hexfall.Grid;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private GridSpawner gridSpawner;

        private Hexagon selectedHexagon;
        private bool isHexagonSelected = false;

        public void Initialize(GridSpawner gridSpawner)
        {
            playerInput.Initialize();
            this.gridSpawner = gridSpawner;
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
                        Debug.Log($"Selected Hexagon Type: {selectedHexagon.HexagonType}, IndexX : {selectedHexagon.IndexX}, IndexY : {selectedHexagon.IndexY}");
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
                    Debug.Log($"Still selecting Hexagon Type: {selectedHexagon.HexagonType}, IndexX : {selectedHexagon.IndexX}, IndexY : {selectedHexagon.IndexY}");
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
            var hexCenter = selectedHexagon.transform.position;
            var halfColliderSizeX = selectedHexagon.GetColliderBoundsSize().x / 2f;
            var halfColliderSizeY = selectedHexagon.GetColliderBoundsSize().y / 2f;

            var isRight = inputPosition.x > hexCenter.x;
            var isAbove = inputPosition.y > hexCenter.y;

            var offsetX = Mathf.Abs(inputPosition.x - hexCenter.x) / halfColliderSizeX;
            var centerPointY = isAbove ? (offsetX * halfColliderSizeY) + hexCenter.y : hexCenter.y - (offsetX * halfColliderSizeY);

            CalculateOtherTwoHexagons(inputPosition.y, centerPointY, isAbove, isRight);
        }

        private void CalculateOtherTwoHexagons(float inputY, float centerPointY, bool isAbove, bool isRight)
        {
            var firstX = selectedHexagon.IndexX;
            var firstY = selectedHexagon.IndexY;

            Vector2 secondHex, thirdHex;

            if (isAbove && isRight) // top right area of the hexagon
            {
                secondHex = inputY > centerPointY ? new Vector2(firstX, firstY + 1) : new Vector2(firstX + 1, firstY);
                thirdHex = inputY > centerPointY ? new Vector2(firstX + 1, firstY) : new Vector2(firstX + 1, firstY - 1);
            }
            else if (!isAbove && isRight) // bottom right area of the hexagon
            {
                secondHex = inputY > centerPointY ? new Vector2(firstX + 1, firstY) : new Vector2(firstX + 1, firstY - 1);
                thirdHex = inputY > centerPointY ? new Vector2(firstX + 1, firstY - 1) : new Vector2(firstX, firstY - 1);
            }
            else if (!isAbove) // bottom left area of the hexagon
            {
                secondHex = new Vector2(firstX - 1, firstY - 1);
                thirdHex = inputY > centerPointY ? new Vector2(firstX - 1, firstY) : new Vector2(firstX, firstY - 1);
            }
            // top left area of the hexagon
            else
            {
                secondHex = inputY > centerPointY ? new Vector2(firstX - 1, firstY) : new Vector2(firstX - 1, firstY - 1);
                thirdHex = inputY > centerPointY ? new Vector2(firstX, firstY + 1) : new Vector2(firstX - 1, firstY);
            }

            SelectOtherTwoHexagons(secondHex, thirdHex);
        }

        private void SelectOtherTwoHexagons(Vector2 secondHex, Vector2 thirdHex)
        {
            var secondHexagon = gridSpawner.GetHexagonObject((int)secondHex.x, (int)secondHex.y);
            var thirdHexagon = gridSpawner.GetHexagonObject((int)thirdHex.x, (int)thirdHex.y);
            Debug.Log($"secondHexagonType {secondHexagon.HexagonType}, secondX {secondHexagon.IndexX}, secondY {secondHexagon.IndexY}");
            Debug.Log($"threeHexagonType {thirdHexagon.HexagonType} thirdX {thirdHexagon.IndexX}, thirdY {thirdHexagon.IndexY}");
        }
    }
}