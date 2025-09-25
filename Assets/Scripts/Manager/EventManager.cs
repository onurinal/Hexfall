using System;
using System.Collections.Generic;
using Hexfall.Hex;

namespace Hexfall.Manager
{
    public static class EventManager
    {
        public static Action<List<Hexagon>> OnScoreChanged;
        public static Action OnMoveChanged;
        public static Action OnGameOver;

        public static void StartOnScoreChangedEvent(List<Hexagon> hexagons)
        {
            OnScoreChanged?.Invoke(hexagons);
        }

        public static void StartOnMoveChangedEvent()
        {
            OnMoveChanged?.Invoke();
        }

        public static void StartOnGameOverEvent()
        {
            OnGameOver?.Invoke();
        }
    }
}