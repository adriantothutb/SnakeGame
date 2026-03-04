using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
    enum Direction // pridanie enum typu
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
            Direction direction = Direction.Right; // zmena typu premennej
            List<int> bodyX = new List<int>();
            List<int> bodyY = new List<int>();
            int berryX = random.Next(0, screenWidth);
            int berryY = random.Next(0, screenHeight);
            DateTime frameStartTime = DateTime.Now;
            DateTime currentTime = DateTime.Now;
            bool buttonPressed = false;
            while (true)
            {
                Console.Clear();
                if (head.XPos == screenWidth-1 || head.XPos == 0 ||head.YPos == screenHeight-1 || head.YPos == 0)
                { 
                    gameOver = true;
                }
                for (int i = 0;i< screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight -1);
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
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryX == head.XPos && berryY == head.YPos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth-2);
                    berryY = random.Next(1, screenHeight-2);
                } 
                for (int i = 0; i < bodyX.Count; i++)
                {
                    Console.SetCursorPosition(bodyX[i], bodyY[i]);
                    Console.Write("■");
                    if (bodyX[i] == head.XPos && bodyY[i] == head.YPos)
                    {
                        gameOver = true;
                    }
                }
                if (gameOver)
                {
                    break;
                }
                Console.SetCursorPosition(head.XPos, head.YPos);
                Console.ForegroundColor = head.ConsoleColor;
                Console.Write("■");
                Console.SetCursorPosition(berryX, berryY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                frameStartTime = DateTime.Now;
                buttonPressed = false;
                while (true)
                {
                    currentTime = DateTime.Now;
                    if (currentTime.Subtract(frameStartTime).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key.Equals(ConsoleKey.UpArrow) && direction != Direction.Down && !buttonPressed) // zmena podmienok pri stlačení klávesy
                        {
                            direction = Direction.Up;
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.DownArrow) && direction != Direction.Up && !buttonPressed)
                        {
                            direction = Direction.Down;
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.LeftArrow) && direction != Direction.Right && !buttonPressed)
                        {
                            direction = Direction.Left;
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.RightArrow) && direction != Direction.Left && !buttonPressed)
                        {
                            direction = Direction.Right;
                            buttonPressed = true;
                        }
                    }
                }
                bodyX.Add(head.XPos);
                bodyY.Add(head.YPos);
                switch (direction) // zmena switchu podľa enum typov
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
        class Pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ConsoleColor { get; set; }
        }
    }
}

