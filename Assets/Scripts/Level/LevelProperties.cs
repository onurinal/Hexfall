using UnityEngine;

namespace Hexfall.Level
{
    [CreateAssetMenu(fileName = "Level 1", menuName = "Hexfall/Create New LevelProperties")]
    public class LevelProperties : ScriptableObject
    {
        [SerializeField] private int gridWidth, gridHeight;

        public int GridWidth => gridWidth;
        public int GridHeight => gridHeight;
    }
}