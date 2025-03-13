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

                if (hexagonGrid[startX, startY].HexagonType == hexagonGrid[secondX, secondY].HexagonType)
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
                    if (hexagonGrid[startX, startY].HexagonType != hexagonGrid[secondX + addX, secondY + addY].HexagonType) return;

                    matchCount++;
                    AddHexagonInTempMatchList(hexagonGrid[secondX + addX, secondY + addY]);

                    if (matchCount >= MatchThreshold)
                    {
                        foreach (var hexagon in tempMatchList)
                        {
                            AddHexagonInMatchList(hexagon);
                            Debug.Log($"HexagonType: {hexagon.HexagonType}, X: {hexagon.IndexX}, Y: {hexagon.IndexY}, Direction: {direction}");
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
    }
}