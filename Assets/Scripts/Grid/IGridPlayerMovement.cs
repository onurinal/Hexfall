using System.Collections;
using Hexfall.Hex;
using UnityEngine;

namespace Hexfall.Grid
{
    public interface IGridPlayerMovement
    {
        IEnumerator StartSwapHexagons(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, Vector2 moveDirection, Vector2 currentInputPosition);
    }
}