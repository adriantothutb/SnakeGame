using System;

namespace Snake
{
    class Program
    {
        const char Block = '■';
        const int WindowHeight = 16;
        const int WindowWidth = WindowHeight * 2;
        const int InitialScore = 5;
        const int FrameDelay = 500;

        static void Main(string[] args)
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            var state = new GameState(WindowWidth, WindowHeight, new Random())
            {
                Score = InitialScore,
                Head = new Position(WindowWidth / 2, WindowHeight / 2),
                Direction = Direction.Right
            };

            var game = new SnakeGame(state);

            game.SpawnBerry();
 
            while (!state.GameOver)
            {
                Render(state);
                state.Direction = HandleInput(state.Direction);
                game.Update();
            }
            Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + state.Score);
        }

        private static void Render(GameState state)
        {
            Console.Clear();
            DrawBorder(state);
            DrawSnakeBody(state);
            DrawBerry(state);
            DrawHead(state);
        }

        private static void DrawBorder(GameState state)
        {
            for (int i = 0; i < state.ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.SetCursorPosition(i, state.ScreenHeight - 1);
                Console.Write(Block);
                Console.ResetColor();
            }

            for (int i = 0; i < state.ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.SetCursorPosition(state.ScreenWidth - 1, i);
                Console.Write(Block);
            }
        }

        private static void DrawSnakeBody(GameState state)
        {
            foreach (var part in state.Body)
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.ResetColor();
            }
        }
        
        private static void DrawHead(GameState state)
        {
            Console.SetCursorPosition(state.Head.X, state.Head.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Block);
            Console.ResetColor();
        }

        private static void DrawBerry(GameState state)
        {
            Console.SetCursorPosition(state.Berry.X, state.Berry.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Block);
            Console.ResetColor();
        }

        private static Direction HandleInput(Direction direction)
        {
            DateTime frameStartTime = DateTime.Now;
            bool buttonPressed = false;

            while (true)
            {
                if ((DateTime.Now - frameStartTime).TotalMilliseconds > FrameDelay)
                    break;

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.UpArrow && direction != Direction.Down && !buttonPressed)
                    {
                        direction = Direction.Up;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.DownArrow && direction != Direction.Up && !buttonPressed)
                    {
                        direction = Direction.Down;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.LeftArrow && direction != Direction.Right && !buttonPressed)
                    {
                        direction = Direction.Left;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.RightArrow && direction != Direction.Left && !buttonPressed)
                    {
                        direction = Direction.Right;
                        buttonPressed = true;
                    }
                }
            }
            return direction;
        }
    }
}