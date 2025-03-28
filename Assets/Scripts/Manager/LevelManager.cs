using System.Collections;
using Hexfall.CameraManager;
using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Player;
using UnityEngine;

namespace Hexfall.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelProperties levelProperties;

        private GridSpawner gridSpawner;
        private GridChecker gridChecker;
        private GridMovement gridMovement;
        private HexagonProperties hexagonProperties;

        private IEnumerator scanGridCoroutine;
        private IEnumerator newLevelCoroutine;

        public bool IsGridInitializing { get; private set; } = true;

        public void Initialize(CameraController cameraController, PlayerController playerController, HexagonProperties hexagonProperties, Transform hexagonParent)
        {
            this.hexagonProperties = hexagonProperties;
            cameraController.Initialize(levelProperties, hexagonProperties);
            gridSpawner = new GridSpawner();
            gridChecker = new GridChecker();
            gridMovement = new GridMovement();
            gridSpawner.Initialize(this, gridChecker, gridMovement, levelProperties, hexagonProperties, hexagonParent, cameraController);
            playerController.Initialize(gridSpawner, gridMovement, levelProperties, hexagonProperties);

            StartNewLevelCoroutine();
        }

        private IEnumerator ScanGridCoroutine()
        {
            var moveDuration = IsGridInitializing ? 0 : hexagonProperties.MoveDuration;
            do
            {
                gridChecker.CheckAllGrid();
                gridChecker.DestroyHexagonInMatchList();
                yield return CoroutineHandler.Instance.StartCoroutine(gridMovement.StartFillHexagonEmptySlot(moveDuration));
                yield return CoroutineHandler.Instance.StartCoroutine(gridSpawner.StartCreateNewHexagonToEmptySlot(moveDuration));
                gridChecker.CheckAllGrid();
            } while (gridChecker.GetMatchListCount() > 0); // Debug.Log("Checking all grid");

            scanGridCoroutine = null;
        }

        public IEnumerator StartScanGrid()
        {
            if (scanGridCoroutine != null) yield break;

            scanGridCoroutine = ScanGridCoroutine();
            yield return scanGridCoroutine;
        }

        private void StopScanGrid()
        {
            if (scanGridCoroutine != null)
            {
                CoroutineHandler.Instance.StopCoroutine(scanGridCoroutine);
                scanGridCoroutine = null;
            }
        }

        private IEnumerator NewLevelCoroutine()
        {
            gridSpawner.HideAllHexagons();
            yield return CoroutineHandler.Instance.StartCoroutine(StartScanGrid());
            gridMovement.MoveAllHexagonsToTheTop();
            yield return gridMovement.StartFallHexagonsCoroutine(hexagonProperties.MoveDuration);
            gridSpawner.ShowAllHexagons();
            IsGridInitializing = false;
            newLevelCoroutine = null;
        }

        private void StartNewLevelCoroutine()
        {
            if (newLevelCoroutine != null) return;
            newLevelCoroutine = NewLevelCoroutine();
            CoroutineHandler.Instance.StartCoroutine(NewLevelCoroutine());
        }

        private void StopNewLevelCoroutine()
        {
            if (newLevelCoroutine == null) return;

            CoroutineHandler.Instance.StopCoroutine(newLevelCoroutine);
            newLevelCoroutine = null;
        }
    }
}