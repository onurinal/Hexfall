using UnityEngine;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Player;
using Hexfall.CameraManager;

namespace Hexfall.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private HexagonProperties hexagonProperties;
        [SerializeField] private Transform hexagonParent;

        private void Start()
        {
            uiManager.Initialize();
            levelManager.Initialize(cameraController, playerController, hexagonProperties, hexagonParent);
        }
    }
}