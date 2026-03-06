using System;
using System.Collections.Generic;

namespace Snake
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        const char Block = '■';
        const int WindowHeight = 16;   // výška herného okna
        const int WindowWidth = WindowHeight * 2;  // šírka je dvojnásobná kvôli obdĺžnikovým bunkám konzoly
        const int InitialScore = 5;    // počiatočná dĺžka hada
        const int FrameDelay = 500;    // čas medzi pohybmi hada (ms)

        private class GameState
        {
            public int ScreenWidth { get; }
            public int ScreenHeight { get; }

            public Random Random { get; }

            public int Score { get; set; }
            public bool GameOver { get; set; }

            public Position Head { get; set; }
            public ConsoleColor HeadColor { get; set; }

            public Direction Direction { get; set; }

            public List<Position> Body { get; } = new List<Position>();
            public Position Berry { get; set; }

            public GameState(int screenWidth, int screenHeight, Random random)
            {
                ScreenWidth = screenWidth;
                ScreenHeight = screenHeight;
                Random = random;
            }
        }

        static void Main(string[] args)
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            GameState state = new GameState(WindowWidth, WindowHeight, new Random()); // rozmery hry sú dané konštantami
            state.Score = InitialScore;
            state.GameOver = false;

            state.Head = new Position(WindowWidth / 2, WindowHeight / 2);
            state.HeadColor = ConsoleColor.Red;

            state.Direction = Direction.Right;

            state.Berry = SpawnBerry(state.Random, state.ScreenWidth, state.ScreenHeight);
 
            while (true)
            {
                Render(state); // vykreslenie je oddelené od logiky hry

                if (state.GameOver)
                {
                    break;
                }

                state.Direction = HandleInput(state.Direction);
                Update(state); // zmena stavu hry je na jednom mieste
            }
            Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + state.Score);
            Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2 + 1);
        }

        private static void Render(GameState state) // vykresľovanie je oddelené od aktualizácie herného stavu
        {
            Console.Clear();

            DrawBorder(state.ScreenWidth, state.ScreenHeight);
            DrawSnakeBody(state.Body);
            DrawHead(state.Head, state.HeadColor);
            DrawBerry(state.Berry);

            if (state.GameOver)
            {
                Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2);
                Console.WriteLine("Game over, Score: " + state.Score);
                Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2 + 1);
            }
        }

        private static void Update(GameState state) // všetky pravidlá hry sú sústredené na jednom mieste
        {
            if (HitWall(state.Head, state.ScreenWidth, state.ScreenHeight))
            {
                state.GameOver = true;
                return;
            }

            TryEatBerry(state);

            if (HitBody(state.Body, state.Head)) // kolíziu kontroluje logická metóda, kreslenie je oddelené
            {
                state.GameOver = true;
                return;
            }

            state.Body.Add(state.Head); // uloží sa predchádzajúca pozícia hlavy do tela hada

            state.Head = MoveSnake(state.Head, state.Direction);

            if (state.Body.Count > state.Score)
            {
                state.Body.RemoveAt(0);
            }
        }

        private static void DrawBorder(int screenWidth, int screenHeight)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(Block);
            }

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write(Block);
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(Block);
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write(Block);
            }
        }

        private static void DrawSnakeBody(List<Position> body) // kreslenie tela hada je oddelené od logiky kolízie
        {
            for (int i = 0; i < body.Count; i++)
            {
                Console.SetCursorPosition(body[i].X, body[i].Y);
                Console.Write(Block);
            }
        }

        private static bool HitBody(List<Position> body, Position head) // kolízia s telom je logika hry, nie vykresľovanie
        {
            for (int i = 0; i < body.Count; i++)
            {
                if (body[i].X == head.X && body[i].Y == head.Y)
                {
                    return true;
                }
            }

            return false;
        }
        
        private static void DrawHead(Position head, ConsoleColor headColor)
        {
            Console.SetCursorPosition(head.X, head.Y);
            Console.ForegroundColor = headColor;
            Console.Write(Block);
        }

        private static void DrawBerry(Position berry)
        {
            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Block);
        }

        private static Position SpawnBerry(Random random, int screenWidth, int screenHeight)
        {
            int x = random.Next(1, screenWidth - 2);
            int y = random.Next(1, screenHeight - 2);
            return new Position(x, y);
        }

        private static void TryEatBerry(GameState state)
        {
            if (state.Head.X == state.Berry.X && state.Head.Y == state.Berry.Y)
            {
                state.Score++;
                state.Berry = SpawnBerry(state.Random, state.ScreenWidth, state.ScreenHeight);
            }
        }

        private static Direction HandleInput(Direction direction)
        {
            DateTime frameStartTime = DateTime.Now;
            bool buttonPressed = false;

            while (true)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Subtract(frameStartTime).TotalMilliseconds > FrameDelay) // použitie konštanty namiesto magic number
                {
                    break;
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

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

        private static Position MoveSnake(Position head, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Position(head.X, head.Y - 1);

                case Direction.Down:
                    return new Position(head.X, head.Y + 1);

                case Direction.Left:
                    return new Position(head.X - 1, head.Y);

                case Direction.Right:
                    return new Position(head.X + 1, head.Y);

                default:
                    return head;
            }
        }

        private static bool HitWall(Position head, int screenWidth, int screenHeight)
        {
            return head.X == screenWidth - 1 || head.X == 0 || head.Y == screenHeight - 1 || head.Y == 0;
        }
    }
}