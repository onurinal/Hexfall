using System;
using System.Collections.Generic;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Manager
{
    public static class EventManager
    {
        public static Action<List<Hexagon>> OnScoreChanged;
        public static Action OnMoveChanged;

        public static void StartOnScoreChangedEvent(List<Hexagon> hexagons)
        {
            OnScoreChanged?.Invoke(hexagons);
        }

        public static void StartOnMoveChangedEvent()
        {
            OnMoveChanged?.Invoke();
        }
    }
}