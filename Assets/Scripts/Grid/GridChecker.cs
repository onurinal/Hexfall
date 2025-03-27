using System;
using System.Collections.Generic;
using Hexfall.Hex;
using Hexfall.Level;
using UnityEngine;

namespace Hexfall.Grid
{
    public class GridChecker
    {
        private const int MatchThreshold = 3;

        private Hexagon[,] hexagonGrid;
        private readonly List<Hexagon> matchList = new List<Hexagon>();
        private readonly List<Hexagon> tempMatchList = new List<Hexagon>();

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
                var secondY = direction == DirectionType.Horizontal ? startY : i + 1;

                int addX = 0, addY = 0;
                if (i + 1 >= gridSize) return; // fix boundary issue

                if (IsHexagonNull(startX, startY) || IsHexagonNull(secondX, secondY)) return;

                if (hexagonGrid[startX, startY].HexagonColorType == hexagonGrid[secondX, secondY].HexagonColorType)
                {
                    matchCount++;
                    AddHexagonInTempMatchList(hexagonGrid[secondX, secondY]);

                    if (direction == DirectionType.Horizontal && startX % 2 == 0)
                    {
                        addX = 0;
                        addY = 1;
                    }
                    else if (direction == DirectionType.Horizontal && startX % 2 != 0)
                    {
                        addX = 0;
                        addY = -1;
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

            foreach (var hexagon in matchList)
            {
                if (hexagon == null) return;

                hexagon.DestroyHexagon();
                hexagonGrid[hexagon.IndexX, hexagon.IndexY] = null;
            }

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
                            // AddAllSameColorHexagonInMatchList(hexagon.HexagonColorType);
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
                if (hexagon.HexagonColorType == hexagonColor)
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

        public int ScanGridAndGetMatchListCount()
        {
            CheckAllGrid();
            return matchList.Count;
        }
    }
}