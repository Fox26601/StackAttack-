using System;
using System.Collections.Generic;

// Represents a single box in the game
class Box
{
    private int x;
    private int y;

    // Static list of all boxes
    private static List<Box> boxes = new List<Box>();

    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        boxes.Add(this);
    }

    public int GetX() { return x; }
    public int GetY() { return y; }
    public void SetX(int newX) { x = newX; }
    public void SetY(int newY) { y = newY; }

    public static void SpawnBox()
    {
        Random rand = new Random();
        int x = rand.Next(1, GridManager.Width - 2);
        new Box(x, 1);
    }

    public static void UpdateBoxes()
    {
        foreach (Box box in boxes)
        {
            if (!IsBoxAt(box.GetX(), box.GetY() + 1) && box.GetY() < GridManager.Height - 2)
            {
                box.SetY(box.GetY() + 1);
            }
        }
    }

    public static bool IsBoxAt(int checkX, int checkY)
    {
        foreach (Box box in boxes)
        {
            if (box.GetX() == checkX && box.GetY() == checkY)
            {
                return true;
            }
        }
        return false;
    }

    public static void RemoveRow(int rowIndex)
    {
        List<Box> remainingBoxes = new List<Box>();
        foreach (Box box in boxes)
        {
            if (box.GetY() != rowIndex)
            {
                remainingBoxes.Add(box);
            }
        }
        boxes = remainingBoxes;
    }

    public static List<Box> GetAllBoxes()
    {
        return boxes;
    }

    public static void ClearAllBoxes()
    {
        boxes.Clear();
    }
}