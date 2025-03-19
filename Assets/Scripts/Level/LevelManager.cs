using System.Collections;
using DefaultNamespace;
using Hexfall.CameraManager;
using Hexfall.Grid;
using Hexfall.Hex;
using Hexfall.Player;
using UnityEngine;

namespace Hexfall.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelProperties levelProperties;

        private GridSpawner gridSpawner;
        private GridChecker gridChecker;
        private GridMovement gridMovement;

        private IEnumerator scanGridCoroutine;

        public void Initialize(CameraController cameraController, PlayerController playerController, HexagonProperties hexagonProperties, Transform hexagonParent)
        {
            cameraController.Initialize(levelProperties, hexagonProperties);
            gridSpawner = new GridSpawner();
            gridChecker = new GridChecker();
            gridMovement = new GridMovement();
            gridSpawner.Initialize(gridChecker, gridMovement, levelProperties, hexagonProperties, hexagonParent, cameraController);
            playerController.Initialize(gridSpawner, levelProperties);

            StartScanGrid();
        }

        private IEnumerator ScanGridCoroutine()
        {
            do
            {
                yield return new WaitForSeconds(0.2f);
                gridChecker.DestroyHexagonInMatchList();
                // Debug.Log("Destroyed");
                yield return new WaitForSeconds(0.2f);
                yield return gridMovement.StartFillHexagonEmptySlot();
                yield return new WaitForSeconds(0.2f);
                // Debug.Log("Starting fill");
                yield return gridSpawner.StartCreateNewHexagonToEmptySlot();
                yield return new WaitForSeconds(0.2f);
                // Debug.Log("Starting create new hexagon");
                gridChecker.CheckAllGrid();
                yield return new WaitForSeconds(0.2f);
                // Debug.Log("Checking all grid");
            } while (gridChecker.GetMatchListCount() > 0);

            scanGridCoroutine = null;
        }

        private void StartScanGrid()
        {
            if (scanGridCoroutine != null) return;

            scanGridCoroutine = ScanGridCoroutine();
            CoroutineHandler.Instance.StartCoroutine(scanGridCoroutine);
        }

        private void StopScanGrid()
        {
            if (scanGridCoroutine != null)
            {
                CoroutineHandler.Instance.StopCoroutine(scanGridCoroutine);
                scanGridCoroutine = null;
            }
        }
    }
}