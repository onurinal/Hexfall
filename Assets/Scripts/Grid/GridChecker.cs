using System;
using System.Collections.Generic;
using Hexfall.Hex;
using Hexfall.Level;
using Hexfall.Manager;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridChecker
    {
        private const int MatchThreshold = 3;

        private Hexagon[,] hexagonGrid;
        private readonly List<Hexagon> tempMatchList = new List<Hexagon>();
        private readonly List<Hexagon> matchList = new List<Hexagon>();
        private readonly List<List<Hexagon>> matchComboList = new List<List<Hexagon>>();

        private readonly Vector2Int[][] neighbourOffsets = new Vector2Int[][]
        {
            new Vector2Int[] { new(-1, 0), new(-1, 1), new(0, -1), new(0, 1), new(1, 0), new(1, 1), }, // for even rows
            new Vector2Int[] { new(-1, -1), new(-1, 0), new(0, -1), new(0, 1), new(1, 0), new(1, -1), } // for odd rows
        };

        private int gridWidth, gridHeight;

        private enum DirectionType
        {
            Horizontal,
            Vertical
        }

        public void Initialize(Hexagon[,] hexagonGrid, LevelProperties levelProperties)
        {
            this.hexagonGrid = hexagonGrid;

            gridWidth = levelProperties.GridWidth;
            gridHeight = levelProperties.GridHeight;
        }

        public void CheckAllGrid()
        {
            for (int height = 0; height < gridHeight; height++)
            {
                for (int width = 0; width < gridWidth; width++)
                {
                    CheckGridWithDirection(width, height, DirectionType.Horizontal);
                    CheckGridWithDirection(width, height, DirectionType.Vertical);
                }
            }
        }

        private void CheckGridWithDirection(int startX, int startY, DirectionType direction)
        {
            var startIndex = direction == DirectionType.Horizontal ? startX : startY;
            var gridSize = direction == DirectionType.Horizontal ? gridWidth : gridHeight;
            var matchCount = 1;

            tempMatchList.Clear();
            AddHexagonInTempMatchList(hexagonGrid[startX, startY]);

            for (var i = startIndex; i < gridSize; i++)
            {
                var secondX = direction == DirectionType.Horizontal ? i + 1 : startX;
                int secondY;

                if (direction == DirectionType.Horizontal && startX % 2 == 0)
                {
                    secondY = startY;
                }
                else if (direction == DirectionType.Horizontal && startX % 2 != 0)
                {
                    secondY = startY - 1;
                }
                else
                {
                    secondY = i + 1;
                }

                int addX = 0, addY = 0;
                if (i + 1 >= gridSize) return; // fix boundary issue

                if (secondX >= gridWidth || secondX < 0 || secondY >= gridHeight || secondY < 0) return;
                if (IsHexagonNull(startX, startY) || IsHexagonNull(secondX, secondY)) return;

                if (hexagonGrid[startX, startY].HexagonColorType == hexagonGrid[secondX, secondY].HexagonColorType)
                {
                    matchCount++;
                    AddHexagonInTempMatchList(hexagonGrid[secondX, secondY]);

                    if (direction == DirectionType.Horizontal)
                    {
                        addX = 0;
                        addY = 1;
                    }
                    else if (direction == DirectionType.Vertical && startX % 2 == 0)
                    {
                        addX = 1;
                        addY = 0;
                    }
                    else if (direction == DirectionType.Vertical && startX % 2 != 0)
                    {
                        addX = 1;
                        addY = -1;
                    }

                    if (secondX + addX >= gridWidth || secondX + addX < 0 || secondY + addY >= gridHeight || secondY + addY < 0) return;

                    if (IsHexagonNull(startX, startY) || IsHexagonNull(secondX + addX, secondY + addY)) return;
                    if (hexagonGrid[startX, startY].HexagonColorType != hexagonGrid[secondX + addX, secondY + addY].HexagonColorType) return;

                    matchCount++;
                    AddHexagonInTempMatchList(hexagonGrid[secondX + addX, secondY + addY]);

                    if (matchCount >= MatchThreshold)
                    {
                        foreach (var hexagon in tempMatchList)
                        {
                            AddHexagonInMatchList(hexagon);
                        }

                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private bool IsHexagonNull(int width, int height)
        {
            return hexagonGrid[width, height] == null;
        }

        private void AddHexagonInMatchList(Hexagon hexagon)
        {
            if (!matchList.Contains(hexagon))
            {
                matchList.Add(hexagon);
            }
        }

        private void AddHexagonInTempMatchList(Hexagon hexagon)
        {
            if (!tempMatchList.Contains(hexagon))
            {
                tempMatchList.Add(hexagon);
            }
        }

        public void DestroyHexagonInMatchList()
        {
            if (matchList == null) return;

            CheckComboForBombInMatchList();
            GetComboListInMatchList();

            if (matchComboList.Count != 0)
            {
                for (int i = 0; i <= matchComboList.Count - 1; i++)
                {
                    EventManager.StartOnScoreChangedEvent(matchComboList[i]);
                    foreach (var hexagon in matchComboList[i])
                    {
                        if (hexagon == null) return;

                        hexagon.DestroyHexagon();
                        hexagonGrid[hexagon.IndexX, hexagon.IndexY] = null;
                    }
                }
            }

            // foreach (var hexagon in matchList)
            // {
            //     if (hexagon == null) return;
            //
            //     hexagon.DestroyHexagon();
            //     hexagonGrid[hexagon.IndexX, hexagon.IndexY] = null;
            // }

            matchComboList.Clear();
            matchList.Clear();
        }

        private List<HexagonColorType> CheckBombCandyInMatchList()
        {
            List<HexagonColorType> bombColorList = new List<HexagonColorType>();
            foreach (var hexagon in matchList)
            {
                if (hexagon.HexagonType == HexagonType.Special && hexagon.HexagonSpecialType == HexagonSpecialType.Bomb)
                {
                    if (bombColorList.Contains(hexagon.HexagonColorType)) continue;
                    bombColorList.Add(hexagon.HexagonColorType);
                }
            }

            return bombColorList;
        }

        private void CheckComboForBombInMatchList()
        {
            var bombColorList = CheckBombCandyInMatchList();
            if (bombColorList == null || bombColorList.Count <= 0) return;

            List<HexagonColorType> newComboColorList = new List<HexagonColorType>();

            foreach (var hexagon in matchList)
            {
                if (hexagon.HexagonType == HexagonType.Bonus)
                {
                    foreach (var bombColor in bombColorList)
                    {
                        if (hexagon.HexagonColorType == bombColor)
                        {
                            newComboColorList.Add(hexagon.HexagonColorType);
                            break;
                        }
                    }
                }
            }

            foreach (var color in newComboColorList)
            {
                AddAllSameColorHexagonInMatchList(color);
            }
        }

        private void AddAllSameColorHexagonInMatchList(HexagonColorType hexagonColor)
        {
            foreach (var hexagon in hexagonGrid)
            {
                if (hexagon.HexagonColorType == hexagonColor && hexagon.HexagonSpecialType != HexagonSpecialType.Bomb)
                {
                    if (!matchList.Contains(hexagon))
                    {
                        matchList.Add(hexagon);
                    }
                }
            }
        }

        public int GetMatchListCount()
        {
            return matchList.Count;
        }

        private void GetComboListInMatchList()
        {
            int comboCount = -1;

            for (int i = 0; i < matchList.Count - 1; i++)
            {
                if (IsItNeighbour(matchList[i], matchList[i + 1]))
                {
                    if (matchList[i].HexagonColorType == matchList[i + 1].HexagonColorType)
                    {
                        if (comboCount == -1 && matchComboList.Count == 0)
                        {
                            matchComboList.Add(new List<Hexagon>());
                            comboCount++;
                        }

                        // Add to the current combo
                        if (!matchComboList[comboCount].Contains(matchList[i]))
                        {
                            matchComboList[comboCount].Add(matchList[i]);
                        }

                        // make sure to add last element in combo
                        if (!matchComboList[comboCount].Contains(matchList[i + 1]))
                        {
                            matchComboList[comboCount].Add(matchList[i + 1]);
                        }
                    }
                }
                else
                {
                    matchComboList.Add(new List<Hexagon>());
                    comboCount++;
                }
            }
        }

        private bool IsItNeighbour(Hexagon first, Hexagon second)
        {
            var rowType = first.IndexX % 2;
            var secondIndices = new Vector2Int(second.IndexX, second.IndexY);

            foreach (var offset in neighbourOffsets[rowType])
            {
                var neighbour = new Vector2Int(first.IndexX + offset.x, first.IndexY + offset.y);
                if (secondIndices.Equals(neighbour))
                {
                    return true;
                }
            }

            return false;
        }

        public int ScanGridAndGetMatchListCount()
        {
            CheckAllGrid();
            return matchList.Count;
        }
    }
}