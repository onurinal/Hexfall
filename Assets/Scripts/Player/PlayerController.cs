using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Manager;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerHighlight playerHighlight;
        [SerializeField] private PlayerMovement playerMovement;

        private HexagonProperties hexagonProperties;
        private LevelManager levelManager;

        // private int gridWidth, gridHeight;

        public void Initialize(LevelManager levelManager, GridSpawner gridSpawner, GridMovement gridMovement, LevelProperties levelProperties, HexagonProperties hexagonProperties)
        {
            this.levelManager = levelManager;
            this.hexagonProperties = hexagonProperties;

            playerInput.Initialize();
            playerMovement.Initialize(gridSpawner, gridMovement, playerHighlight, playerInput, levelProperties);
            playerHighlight.Initialize(playerMovement, this.hexagonProperties);

            gridMovement.InitializePlayerHighlight(playerHighlight);
        }

        private void Update()
        {
            if (levelManager.IsGridInitializing) return;

            playerInput.UpdatePlayerInput();
            playerMovement.HandleHexagonSelect();
        }
    }
}