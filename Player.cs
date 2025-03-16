
class Player
{
    private GridManager grid;
    private int x;
    private int y;
    private bool isGrounded;
    private int jumpHeight;
    private bool canJump;
    private GameManager gameManager;
    private DateTime lastKeyPressTime = DateTime.MinValue;
    private TimeSpan keyPressCooldown = TimeSpan.FromMilliseconds(200);

    public Player(GridManager grid, GameManager gameManager)
    {
        this.grid = grid;
        this.gameManager = gameManager;
        x = GridManager.Width / 2; 
        y = GridManager.Height - 2;
        isGrounded = true;
        canJump = true;
    }

    public int GetX() { return x; }
    public int GetY() { return y; }

    public void Update()
    {
        if (Console.KeyAvailable)
        {
            DateTime currentTime = DateTime.Now;

            if ((currentTime - lastKeyPressTime) >= keyPressCooldown)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                lastKeyPressTime = currentTime;

                if (key == ConsoleKey.A)
                {
                    MoveBoxOrPlayer(-1);
                }
                if (key == ConsoleKey.D)
                {
                    MoveBoxOrPlayer(1);
                }
                if (key == ConsoleKey.W && isGrounded)
                {
                    jumpHeight = 2;
                    isGrounded = false;
                    canJump = false;
                }
                if (key == ConsoleKey.Q)
                {
                    gameManager.RestartGame();
                }
            }
        }

        ApplyJump();
        ApplyGravity();
    }

    private void ApplyJump()
    {
        if (jumpHeight > 0)
        {
            y -= 1;
            jumpHeight--;
        }
        else
        {
            isGrounded = true;
        }
    }

    private void ApplyGravity()
    {
        if (jumpHeight == 0 && !Box.IsBoxAt(x, y + 1) && y < GridManager.Height - 2)
        {
            y += 1;
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
            canJump = true;
        }
    }

    private void MoveBoxOrPlayer(int direction)
    {
        int newPlayerX = x + direction;

        if (Box.IsBoxAt(newPlayerX, y))
        {
            Box boxToMove = null;
            foreach (Box box in Box.GetAllBoxes())
            {
                if (box.GetX() == newPlayerX && box.GetY() == y)
                {
                    boxToMove = box;
                    break;
                }
            }

            if (boxToMove != null)
            {
                int newBoxX = boxToMove.GetX() + direction;

                if (!Box.IsBoxAt(newBoxX, boxToMove.GetY()) && newBoxX > 0 && newBoxX < GridManager.Width - 1)
                {
                    boxToMove.SetX(newBoxX);
                    x = newPlayerX;
                }
            }
        }
        else if (newPlayerX > 0 && newPlayerX < GridManager.Width - 1)
        {
            x = newPlayerX;
        }
    }
}
