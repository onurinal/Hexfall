using Hexfall.CameraManager;
using Hexfall.Hex;
using UnityEngine;
using Hexfall.Level;

namespace Hexfall.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private HexagonProperties hexagonProperties;
        [SerializeField] private Transform hexagonParent;

        private void Start()
        {
            levelManager.Initialize(hexagonProperties, hexagonParent);
        }
    }
}