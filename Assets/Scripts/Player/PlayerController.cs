using Hexfall.Grid;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer groupHighlightSprite;
        [SerializeField] private PlayerInput playerInput;

        private PlayerSelection playerSelection;
        // private int gridWidth, gridHeight;

        public void Initialize(GridSpawner gridSpawner, LevelProperties levelProperties)
        {
            playerInput.Initialize();
            playerSelection = new PlayerSelection();
            playerSelection.Initialize(gridSpawner, playerInput, levelProperties, groupHighlightSprite);
        }

        private void Update()
        {
            playerInput.UpdatePlayerInput();
            playerSelection.HandleHexagonSelect();
        }
    }
}