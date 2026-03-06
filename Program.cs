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
        const char Block = '■'; // spoločný symbol pre hada, telo aj rámik (odstránenie duplicity znaku v kóde)

        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random random = new Random();
            int score = 5;
            bool gameOver = false;
            Position head = new Position(screenWidth / 2, screenHeight / 2); // hlava hada je len pozícia (X,Y), nie je potreba osobitnej triedy
            ConsoleColor headColor = ConsoleColor.Red; // farbu sa drží samostatne, aby Position zostal čistý (iba súradnice)
            Direction direction = Direction.Right;
            List<Position> body = new List<Position>();
            Position berry = SpawnBerry(random, screenWidth, screenHeight);
 
            while (true)
            {
                Console.Clear();
                if (HitWall(head, screenWidth, screenHeight))
                {
                    gameOver = true;
                }

                DrawBorder(screenWidth, screenHeight);
                
                TryEatBerry(head, ref berry, random, screenWidth, screenHeight, ref score);
                
                if (DrawSnakeBody(body, head))
                {
                    gameOver = true;
                }
                
                if (gameOver)
                {
                    break;
                }
                
                DrawHead(head, headColor); // vykreslenie hlavy - pozícia + farba
                DrawBerry(berry);
                
                direction = HandleInput(direction);

                body.Add(head); // uloží sa predchádzajúca pozícia hlavy do tela hada

                head = MoveSnake(head, direction); // hlava sa posunie a výsledná pozícia sa uloží späť

                if (body.Count > score)
                {
                    body.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
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

        private static bool DrawSnakeBody(List<Position> body, Position head) // hlava je teraz len Position, Pixel sa už nepoužíva
        {
            for (int i = 0; i < body.Count; i++)
            {
                Console.SetCursorPosition(body[i].X, body[i].Y);
                Console.Write(Block);

                if (body[i].X == head.X && body[i].Y == head.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private static void DrawHead(Position head, ConsoleColor headColor) // farba je mimo pozície, aby Position ostal "len súradnice"
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

        private static void TryEatBerry(Position head, ref Position berry, Random random, int screenWidth, int screenHeight, ref int score) // hlava je pozícia (X,Y)
        {
            if (head.X == berry.X && head.Y == berry.Y) // ak hlava stojí na bobuli, tak ju zjedol
            {
                score++;
                berry = SpawnBerry(random, screenWidth, screenHeight); // presun bobule na nové miesto
            }
        }

        private static Direction HandleInput(Direction direction)
        {
            DateTime frameStartTime = DateTime.Now;
            bool buttonPressed = false;

            while (true)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Subtract(frameStartTime).TotalMilliseconds > 500)
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

        private static Position MoveSnake(Position head, Direction direction) // vracia sa nová pozícia, nemení sa vstup (ľahšie sa to číta)
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
                    return head; // poistka, keby pribudol nový smer
            }
        }

        private static bool HitWall(Position head, int screenWidth, int screenHeight) // hlava je pozícia, nie je potreba Pixelu
        {
            return head.X == screenWidth - 1 || head.X == 0 || head.Y == screenHeight - 1 || head.Y == 0;
        }
        
        // zmazaná trieda Pixel
    }
}