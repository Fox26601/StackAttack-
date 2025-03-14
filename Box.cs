class Box
{
    private int x;
    private int y;

    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Getter Methods
    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    // Setter Methods
    public void SetX(int newX)
    {
        x = newX;
    }

    public void SetY(int newY)
    {
        y = newY;
    }
}