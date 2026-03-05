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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random random = new Random();
            int score = 5;
            bool gameOver = false;
            Pixel head = new Pixel();
            head.XPos = screenWidth / 2;
            head.YPos = screenHeight / 2;
            head.ConsoleColor = ConsoleColor.Red;
            Direction direction = Direction.Right;
            List<int> bodyX = new List<int>();
            List<int> bodyY = new List<int>();
            int berryX = random.Next(0, screenWidth);
            int berryY = random.Next(0, screenHeight);
 
            while (true)
            {
                Console.Clear();
                if (HitWall(head, screenWidth, screenHeight))  // kontrola kolízie so stenou presunutá do samostatnej metódy
                {
                    gameOver = true;
                }

                DrawBorder(screenWidth, screenHeight);
                
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryX == head.XPos && berryY == head.YPos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth - 2);
                    berryY = random.Next(1, screenHeight - 2);
                } 
                
                if (DrawSnakeBody(bodyX, bodyY, head))
                {
                    gameOver = true;
                }
                
                if (gameOver)
                {
                    break;
                }
                
                DrawHead(head);
                DrawBerry(berryX, berryY);
                
                direction = HandleInput(direction);

                bodyX.Add(head.XPos);
                bodyY.Add(head.YPos);

                MoveSnake(head, direction); // pohyb hada presunutý do samostatnej metódy

                if (bodyX.Count > score)
                {
                    bodyX.RemoveAt(0);
                    bodyY.RemoveAt(0);
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
                Console.Write("■");
            }

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private static bool DrawSnakeBody(List<int> bodyX, List<int> bodyY, Pixel head)
        {
            for (int i = 0; i < bodyX.Count; i++)
            {
                Console.SetCursorPosition(bodyX[i], bodyY[i]);
                Console.Write("■");

                if (bodyX[i] == head.XPos && bodyY[i] == head.YPos)
                {
                    return true;
                }
            }

            return false;
        }

        private static void DrawHead(Pixel head)
        {
            Console.SetCursorPosition(head.XPos, head.YPos);
            Console.ForegroundColor = head.ConsoleColor;
            Console.Write("■");
        }

        private static void DrawBerry(int berryX, int berryY)
        {
            Console.SetCursorPosition(berryX, berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
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

        private static void MoveSnake(Pixel head, Direction direction) // pohyb hada oddelený od hlavnej hernej slučky
        {
            switch (direction)
            {
                case Direction.Up:
                    head.YPos--;
                    break;

                case Direction.Down:
                    head.YPos++;
                    break;

                case Direction.Left:
                    head.XPos--;
                    break;

                case Direction.Right:
                    head.XPos++;
                    break;
            }
        }

        private static bool HitWall(Pixel head, int screenWidth, int screenHeight) // kontrola nárazu do steny oddelená od hlavnej logiky hry
        {
            return head.XPos == screenWidth - 1 ||
                head.XPos == 0 ||
                head.YPos == screenHeight - 1 ||
                head.YPos == 0;
        }

        class Pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ConsoleColor { get; set; }
        }
    }
}