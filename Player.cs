class Player
{
    // Player position coordinates
    private int x;
    private int y;

    // Indicates if the player is currently on the ground
    private bool isGrounded;

    // Remaining jump height counter
    private int jumpHeight;

    // Maximum allowed jump height
    private readonly int maxJumpHeight = 2;

    // Reference to the GameManager to handle game-wide actions
    private GameManager GameManager;

    // Timestamp for last key press to prevent key spamming
    private DateTime lastKeyPressTime = DateTime.MinValue;

    // Cooldown period between allowed key presses
    private TimeSpan keyPressCooldown = TimeSpan.FromMilliseconds(200);

    // Constructor initializes player position and state
    public Player()
    {
        x = GridManager.Width / 2; 
        y = GridManager.Height - 2;
        isGrounded = true;
        jumpHeight = 0;
    }

    // Returns player's X coordinate
    public int GetX() { return x; }

    // Returns player's Y coordinate
    public int GetY() { return y; }

    // Updates player state each game tick
    public void Update()
    {
        HandleInput();
        ApplyJump();
        ApplyGravity();
    }

    // Handles keyboard input from player
    private void HandleInput()
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
                else if (key == ConsoleKey.D)
                {
                    MoveBoxOrPlayer(1);
                }
                else if (key == ConsoleKey.W && isGrounded)
                {
                    jumpHeight = maxJumpHeight; // Start jump
                    isGrounded = false;
                }
                else if (key == ConsoleKey.Q)
                {
                    GameManager.RestartGame();
                }
            }
        }
    }

    // Manages jump mechanics, moving player upwards if conditions met
    private void ApplyJump()
    {
        if (jumpHeight > 0 && !Box.IsBoxAt(x, y - 1) && y > 1)
        {
            y -= 1;
            jumpHeight--;
        }
        else
        {
            jumpHeight = 0;
        }
    }

    // Applies gravity to the player, pulling downwards if not grounded
    private void ApplyGravity()
    {
        if (jumpHeight == 0)
        {
            if (!Box.IsBoxAt(x, y + 1) && y < GridManager.Height - 2)
            {
                y += 1;
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }
        }
    }

    // Moves the player or pushes a box if possible
    // Moves the player or pushes a box if possible
    private void MoveBoxOrPlayer(int direction)
    {
        int newPlayerX = x + direction;

        // Check if a box is in the intended position
        if (Box.IsBoxAt(newPlayerX, y))
        {
            Box boxToMove = null;

            // Find the box at the player's intended position
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

                // Check if there's a box stacked above; if so, block movement
                if (Box.IsBoxAt(boxToMove.GetX(), boxToMove.GetY() - 1))
                {
                    // If a box is above, prevent pushing
                    return;
                }

                // Move box if next position is clear and within bounds
                if (!Box.IsBoxAt(newBoxX, boxToMove.GetY()) && newBoxX > 0 && newBoxX < GridManager.Width - 1)
                {
                    boxToMove.SetX(newBoxX);
                    x = newPlayerX;
                }
            }
        }
        // Move player if no box is in the way
        else if (newPlayerX > 0 && newPlayerX < GridManager.Width - 1)
        {
            x = newPlayerX;
        }
    }
}

