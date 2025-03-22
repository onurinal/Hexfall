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
            gridSpawner.Initialize(this, gridChecker, gridMovement, levelProperties, hexagonProperties, hexagonParent, cameraController);
            playerController.Initialize(gridSpawner, gridMovement, levelProperties);

            CoroutineHandler.Instance.StartCoroutine(StartScanGrid());
        }

        private IEnumerator ScanGridCoroutine()
        {
            do
            {
                gridChecker.CheckAllGrid();
                yield return new WaitForSeconds(0.2f);
                gridChecker.DestroyHexagonInMatchList();
                yield return new WaitForSeconds(0.2f);
                yield return CoroutineHandler.Instance.StartCoroutine(gridMovement.StartFillHexagonEmptySlot());
                yield return new WaitForSeconds(0.2f);
                yield return CoroutineHandler.Instance.StartCoroutine(gridSpawner.StartCreateNewHexagonToEmptySlot());
                yield return new WaitForSeconds(0.2f);
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
    }
}