class Player
{
    private GridManager grid;
    private int x;
    private int y;
    private bool isGrounded;
    private int jumpHeight;
    private bool canJump;
    private List<Box> boxes;
    private GameManager gameManager;

    private DateTime lastKeyPressTime = DateTime.MinValue; // Stores last key press time
    private TimeSpan keyPressCooldown = TimeSpan.FromMilliseconds(200); // Delay between key presses

    // Constructor initializes the player's attributes
    public Player(GridManager grid, List<Box> boxes, GameManager gameManager)
    {
        this.grid = grid;
        this.boxes = boxes;
        this.gameManager = gameManager;
        x = GridManager.Width / 2; // Start player in the middle of the grid
        y = GridManager.Height - 2; // Place player near the bottom
        isGrounded = true; // Start on the ground
        canJump = true;
    }

    // Getter methods for player's position
    public int GetX() { return x; }
    public int GetY() { return y; }

    // Setter methods to update player's position
    public void SetX(int newX) { x = newX; }
    public void SetY(int newY) { y = newY; }

    // Handles player input and movement with key spam prevention
    public void Update()
    {
        if (Console.KeyAvailable)
        {
            DateTime currentTime = DateTime.Now;

            // Ensure enough time has passed since the last key press
            if ((currentTime - lastKeyPressTime) >= keyPressCooldown)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                lastKeyPressTime = currentTime; // Update last key press time
                
                if (key == ConsoleKey.A) // Move left
                {
                    MoveBoxOrPlayer(-1);
                }
                else if (key == ConsoleKey.D) // Move right
                {
                    MoveBoxOrPlayer(1);
                }
                else if (key == ConsoleKey.W && isGrounded) // Jump if on the ground
                {
                    jumpHeight = 2;
                    isGrounded = false;
                    canJump = false; // Prevent further jumps mid-air
                }
                else if (key == ConsoleKey.Q) // Restart game
                {
                    gameManager.RestartGame();
                }
            }
        }

        ApplyJump();
        ApplyGravity();
        CheckForDeath();
    }

    // Handles jumping logic
    private void ApplyJump()
    {
        if (jumpHeight > 0) // Continue jumping if height remains
        {
            y -= 1; // Move player up
            jumpHeight--; // Reduce remaining jump height
        }
        else
        {
            isGrounded = true;
        }
    }

    // Applies gravity to the player when mid-air
    private void ApplyGravity()
    {
        if (jumpHeight == 0) // Ensure jump is finished
        {
            if (!IsBoxAt(x, y + 1) && y < GridManager.Height - 2) // Check if there's space below
            {
                y += 1; // Move player down
                isGrounded = false; // Player is falling
            }
            else
            {
                isGrounded = true; // Player has landed
                canJump = true; // Allow next jump
            }
        }
    }

    // Moves the player or pushes a box if possible
    private void MoveBoxOrPlayer(int direction)
    {
        int newPlayerX = x + direction; // Calculate new position

        if (IsBoxAt(newPlayerX, y)) // Check if there's a box in the way
        {
            Box box = GetBoxAt(newPlayerX, y); // Get the box at the target position
            int newBoxX = box.GetX() + direction; // Calculate new box position

            if (!IsBoxAt(newBoxX, box.GetY()) && newBoxX > 0 && newBoxX < GridManager.Width - 1) // Check if box can be moved
            {
                box.SetX(newBoxX); // Move the box
                x = newPlayerX; // Move player into the previous box position
            }
        }
        else if (newPlayerX > 0 && newPlayerX < GridManager.Width - 1) // Ensure player stays within boundaries
        {
            x = newPlayerX; // Move player left or right
        }
    }

    // Checks if a box is present at given coordinates
    private bool IsBoxAt(int checkX, int checkY)
    {
        foreach (var box in boxes)
        {
            if (box.GetX() == checkX && box.GetY() == checkY)
            {
                return true;
            }
        }
        return false;
    }

    // Gets the box at a specific location
    private Box GetBoxAt(int x, int y)
    {
        foreach (var box in boxes)
        {
            if (box.GetX() == x && box.GetY() == y)
            {
                return box;
            }
        }
        return null;
    }

    // Checks if the player is crushed by a falling box
    private void CheckForDeath()
    {
        foreach (var box in boxes)
        {
            if (box.GetX() == x && box.GetY() == y - 1) // If a box lands on the player
            {
                Console.Clear();
                Console.WriteLine("Game Over! Press Q to restart.");
                while (Console.ReadKey(true).Key != ConsoleKey.Q) { } // Wait for restart
                gameManager.RestartGame();
            }
        }
    }
}
