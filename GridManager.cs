using System;
using System.Collections.Generic;

class GridManager
{
    public const  int Width = 20; //const values cannot be changed after compilation, making them safer for constants.
    public const  int Height = 20;
    private char[,] grid = new char[Height, Width];

    public GridManager()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                grid[y, x] = (y == 0 || y == Height - 1 || x == 0 || x == Width - 1) ? '#' : ' ';
            }
        }
    }

    public void DrawGrid(Player player, List<Box> boxes)
    {
        Console.Clear();
        char[,] tempGrid = (char[,])grid.Clone();
        tempGrid[player.GetY(), player.GetX()] = '@';

        foreach (var box in boxes)
        {
            tempGrid[box.GetY(), box.GetX()] = 'O';
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(tempGrid[y, x]);
            }
            Console.WriteLine();
        }
    }
}