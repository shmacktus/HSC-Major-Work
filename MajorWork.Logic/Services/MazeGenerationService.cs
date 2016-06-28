﻿using System.Linq;
using MajorWork.Logic.Models;
using MajorWork.Logic.Helpers;

namespace MajorWork.Logic.Services
{
    /* This service takes the coordinates for a maze, and then returns a completely random maze
    generated by depth first search */

    public class MazeGenerationService
    {
        private readonly Maze _mazeGrid;

        public MazeGenerationService(int userLength, int userWidth, Maze mazeGridUser)
        {
            _mazeGrid = mazeGridUser;
            
            _mazeGrid.Width = userWidth;
            _mazeGrid.Length = userLength;

            _mazeGrid.MazeStack.Add(new Stack
            {
                X = 0,
                Y = 0
            });

            GenerateEmptyGrid();
        
            IterableGeneration(0,0);
            _mazeGrid.MazeGrid.First(a => (a.X == (0) && a.Y == 0)).IsPath = true;
        }

        private void GenerateEmptyGrid()
        {         
            for (int j = 0; j < _mazeGrid.Width; j++)
            {
                for (int k = 0; k < _mazeGrid.Length; k++)
                {
                    _mazeGrid.MazeGrid.Add(new Mazepoints(j, k, false));
                }
            }
        }

        private void IterableGeneration (int x, int y)
        {
            while (_mazeGrid.MazeStack.Count != 0)
            {
                bool statement = false;
                //Generate a random selection of integers
                int[] randDirections = RandomDirections();

                //Choose a direction to explore
                for (int i = 0; i < randDirections.Length; i++)
                {
                    switch (randDirections[i])
                    {
                        case 0:
                            if (x - 2 <= 0)
                                continue;

                            if (_mazeGrid.MazeGrid.First(a => (a.X == (x - 2) && a.Y == y)).IsPath == false)
                            {                      
                                _mazeGrid.MazeGrid.First(a => (a.X == (x - 2) && a.Y == y)).IsPath = true;
                                _mazeGrid.MazeGrid.First(a => (a.X == (x - 1) && a.Y == y)).IsPath = true;

                                x = x - 2;
                                AddValueToStack(x, y);
                                i = 5;
                                statement = true;
                            }
                            break;

                        case 1:
                            if (y + 2 > _mazeGrid.Length - 1)
                                continue;

                            if (_mazeGrid.MazeGrid.First(a => (a.X == x && a.Y == (y + 2))).IsPath == false)
                            {                   
                                _mazeGrid.MazeGrid.First(a => (a.X == (x) && a.Y == (y + 2))).IsPath = true;
                                _mazeGrid.MazeGrid.First(a => (a.X == (x) && a.Y == (y + 1))).IsPath = true;

                                y = y + 2;
                                AddValueToStack(x, y);
                                i = 5;
                                statement = true;
                            }
                            break;

                        case 2:
                            if (x + 2 >= _mazeGrid.Length - 1)
                                continue;

                            if (_mazeGrid.MazeGrid.First(a => (a.X == (x + 2) && a.Y == y)).IsPath == false)
                            {
                                _mazeGrid.MazeGrid.First(a => (a.X == (x + 2) && a.Y == (y))).IsPath = true;
                                _mazeGrid.MazeGrid.First(a => (a.X == (x + 1) && a.Y == (y))).IsPath = true;


                                x = x + 2;
                                AddValueToStack(x, y);
                                i = 5;
                                statement = true;
                            }
                            break;

                        case 3:
                            if (y - 2 <= 0)
                                continue;

                            if (_mazeGrid.MazeGrid.First(a => (a.X == x && a.Y == (y - 2))).IsPath == false)
                            {
                                _mazeGrid.MazeGrid.First(a => (a.X == (x) && a.Y == (y - 1))).IsPath = true;
                                _mazeGrid.MazeGrid.First(a => (a.X == (x) && a.Y == (y - 2))).IsPath = true;

                                y = y - 2;
                                AddValueToStack(x, y);
                                i = 5;
                                statement = true;
                            }
                            break;

                    }
                }
                if (statement == false)
                {
                    var stackLength = _mazeGrid.MazeStack.Count() - 1;

                    x = _mazeGrid.MazeStack[stackLength].X;
                    y = _mazeGrid.MazeStack[stackLength].Y;
                    _mazeGrid.MazeStack.RemoveAt(stackLength);
                }

            }

        }

        

        private void AddValueToStack(int x, int y)
        {
            _mazeGrid.MazeStack.Add(new Stack
            {
                X = x,
                Y = y
            });
        }

        private static int[] RandomDirections()
        {
            int[] randoms = new int[4];

            for (int i = 0; i < randoms.Length; i++)
            {
                randoms[i] = i;
            }       

            for (int i = randoms.Length; i > 1; i--)
            {
                int a = MathRandom.GetRandomNumber(0, i);
                int tmp = randoms[a];
                randoms[a] = randoms[i - 1];
                randoms[i - 1] = tmp;
            }
            return randoms;
        }
    }
}
