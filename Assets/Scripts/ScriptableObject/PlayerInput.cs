using UnityEngine;

namespace Hexfall.Player
{
    [CreateAssetMenu(fileName = "Player Input", menuName = "Hexfall/Create New PlayerInput")]
    public class PlayerInput : ScriptableObject
    {
        public Vector2 CurrentMousePosition { get; private set; }
        public Vector2 CurrentTouchPosition { get; private set; }

        public Vector2 FirstMousePosition { get; set; }
        public Vector2 FirstTouchPosition { get; set; }

        private Camera mainCamera;

        public void Initialize()
        {
            mainCamera = Camera.main;
        }

        public void UpdatePlayerInput()
        {
            if (!mainCamera) return;
#if UNITY_EDITOR
            CurrentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
#elif UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                CurrentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
#endif
        }
    }
}