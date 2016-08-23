﻿using System.Windows;
using System.Windows.Shapes;
using MajorWork.Logic.Models;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

namespace MajorWork.ViewModels
{
    class Draw
    {
        private readonly Maze _maze;
        private readonly Grid _grid;
        private Mazepoints _finalCoords;

        public Draw(Maze mazeGrid, Grid blank, Mazepoints finalCoords)
        {
            _maze = mazeGrid;
            _grid = blank;
            _finalCoords = finalCoords;

            DrawMaze();
            ReadyPlayer();
        }

        public Draw()
        {
            //Do Nothing
        }
        private void DrawMaze()
        {
            foreach (var item in _maze.MazeGrid)
            {
                if (item.IsPath)
                {
                    GenerateRectangle(item);
                }
            }
        }

        private void ReadyPlayer()
        {
            Mazepoints startCoord = new Mazepoints(0, 0, false, true);
            UIElement startRect = DrawRect(76, 175, 80);
            AddChildToGrid(startRect, startCoord);

            //Mazepoints finalCoord = new Mazepoints(8, 8, false);
            UIElement finalRect = DrawRect(244, 67, 54);
            AddChildToGrid(finalRect, _finalCoords);
        }

        public void DrawSolution(List<AStar> solution)
        {
            foreach (var position in solution)
            {
                var tempMazePoint = new Mazepoints(position.X, position.Y, true, false);
                GenerateRectangle2(tempMazePoint);
            }

        }

        public void DrawPath(Mazepoints s)
        {
            if (s.IsSolution)
            {
                var myPath = DrawRect(76, 175, 80);
                AddChildToGrid(myPath, s);
            }

            else
            {
                RemovePath(s);
            }

        }

        public void RemovePath(Mazepoints s)
        {
            var pathToRemove = DrawRect(255, 255, 255);
            AddChildToGrid(pathToRemove, s);
        }

        private void GenerateRectangle(Mazepoints s)
        {
            var myRect = DrawRect(255, 255, 255);
            AddChildToGrid(myRect, s);
        }

        private void GenerateRectangle2(Mazepoints s)
        {
            var myRect = DrawRect(174, 213, 129);
            AddChildToGrid(myRect, s);
        }

        private Rectangle DrawRect(byte r, byte g, byte b)
        {
            Rectangle myRect = new Rectangle();
            myRect.Width = 50;
            myRect.Height = 50;
            SolidColorBrush colourBrush = new SolidColorBrush(Color.FromRgb(r, g, b));

            myRect.Fill = colourBrush;
            return myRect;
        }

        private void AddChildToGrid(UIElement val, Mazepoints s)
        {
            _grid.Children.Add(val);
            Grid.SetRow(val, s.Y);
            Grid.SetColumn(val, s.X);
        }

        private void AddChildToGrid2(UIElement val, Mazepoints s)
        {
            _grid.Children.Add(val);
            Grid.SetRow(val, s.Y);
            Grid.SetColumn(val, s.X);
        }
    }
}
