using System;
using System.Collections.Generic;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Manager
{
    public static class EventManager
    {
        public static Action<List<Hexagon>> OnScoreChanged;

        public static void StartOnScoreChangedEvent(List<Hexagon> hexagons)
        {
            OnScoreChanged?.Invoke(hexagons);
        }
    }
}