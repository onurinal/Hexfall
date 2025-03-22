using Hexfall.Grid;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerHighlight playerHighlight;
        [SerializeField] private PlayerMovement playerMovement;

        // private int gridWidth, gridHeight;

        public void Initialize(GridSpawner gridSpawner, GridMovement gridMovement, LevelProperties levelProperties)
        {
            playerInput.Initialize();
            playerMovement.Initialize(gridSpawner, gridMovement, playerHighlight, playerInput, levelProperties);
            playerHighlight.Initialize(playerMovement);
        }

        private void Update()
        {
            playerInput.UpdatePlayerInput();
            playerMovement.HandleHexagonSelect();
        }
    }
}