using System;

public class GameBoard
{
    private char[,] board;
    private const int rows = 6;
    private const int columns = 7;

    public GameBoard()
    {
        board = new char[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                board[i, j] = ' ';
            }
        }
    }

    public bool DropPiece(int column, char piece)
    {
        if (column >= 0 && column < columns)
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                if (board[i, column] == ' ')
                {
                    board[i, column] = piece;
                    return true;
                }
            }
        }
        return false; // Column is full
    }

    public void PrintBoard()
    {
        Console.WriteLine("\nCurrent state of the board:");
        for (int i = 0; i < rows; i++)
        {
            Console.Write("|");
            for (int j = 0; j < columns; j++)
            {
                Console.Write(board[i, j] + "|");
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', columns * 2 + 1)); // Add a separator line for better visibility
    }

    public bool CheckWinner(char piece)
    {
        // Horizontal check
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns - 3; col++)
            {
                if (board[row, col] == piece && board[row, col + 1] == piece &&
                    board[row, col + 2] == piece && board[row, col + 3] == piece)
                {
                    return true;
                }
            }
        }

        // Vertical check
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows - 3; row++)
            {
                if (board[row, col] == piece && board[row + 1, col] == piece &&
                    board[row + 2, col] == piece && board[row + 3, col] == piece)
                {
                    return true;
                }
            }
        }

        // Diagonal checks
        // Ascending Diagonal Check
        for (int col = 0; col < columns - 3; col++)
        {
            for (int row = 3; row < rows; row++)
            {
                if (board[row, col] == piece && board[row - 1, col + 1] == piece &&
                    board[row - 2, col + 2] == piece && board[row - 3, col + 3] == piece)
                {
                    return true;
                }
            }
        }

        // Descending Diagonal Check
        for (int col = 0; col < columns - 3; col++)
        {
            for (int row = 0; row < rows - 3; row++)
            {
                if (board[row, col] == piece && board[row + 1, col + 1] == piece &&
                    board[row + 2, col + 2] == piece && board[row + 3, col + 3] == piece)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsFull()
    {
        for (int j = 0; j < columns; j++)
        {
            if (board[0, j] == ' ')
            {
                return false; // At least one column is not full
            }
        }
        return true; // All columns are full
    }
}

public abstract class Player
{
    protected char symbol;

    public char Symbol
    {   // Property to access 'symbol'
        get { return symbol; }
    }

    public Player(char symbol)
    {
        this.symbol = symbol;
    }

    public abstract void MakeMove(GameBoard board);
}

public class HumanPlayer : Player
{
    public HumanPlayer(char symbol) : base(symbol) { }

    public override void MakeMove(GameBoard board)
    {
        int column;
        bool validMove = false;
        do
        {
            Console.WriteLine($"Player {Symbol}, it's your turn. Choose column (0-6): ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out column) && column >= 0 && column < 7)
            {
                validMove = board.DropPiece(column, symbol);
                if (!validMove)
                {
                    Console.WriteLine("Column is full! Try another one.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 6.");
            }
        } while (!validMove);
    }
}

public class GameController
{
    private GameBoard board;
    private Player player1, player2;

    public GameController()
    {
        board = new GameBoard();
        player1 = new HumanPlayer('X');
        player2 = new HumanPlayer('O');
    }

    public void StartGame()
    {
        Player currentPlayer = player1;
        bool gameRunning = true;

        while (gameRunning)
        {
            board.PrintBoard();
            currentPlayer.MakeMove(board);
            if (board.CheckWinner(currentPlayer.Symbol))
            {
                Console.WriteLine($"Congratulations! Player {currentPlayer.Symbol} wins!");
                gameRunning = false;
            }
            else if (board.IsFull())
            {
                Console.WriteLine("The board is full - it's a draw!");
                gameRunning = false;
            }
            else
            {
                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
        }
        board.PrintBoard(); // Show final board
    }
}

class Program
{
    static void Main(string[] args)
    {
        GameController game = new GameController();
        game.StartGame();
    }
}
