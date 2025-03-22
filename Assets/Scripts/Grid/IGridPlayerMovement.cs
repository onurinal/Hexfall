using System.Collections;
using Hexfall.Hex;

namespace Hexfall.Grid
{
    public interface IGridPlayerMovement
    {
        IEnumerator StartSwapHexagons(Hexagon firstHex, Hexagon secondHex, Hexagon thirdHex, int angleDiff);

        bool IsSwapping { get; set; }
    }
}